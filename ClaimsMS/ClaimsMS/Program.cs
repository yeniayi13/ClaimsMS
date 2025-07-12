using ClaimsMS;
using ClaimsMS.Application;
using ClaimsMS.Common.AutoMapper;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Common.Dtos.Resolution.Response;
using ClaimsMS.Core.Database;
using ClaimsMS.Core.RabbitMQ;
using ClaimsMS.Core.Service.Auction;
using ClaimsMS.Core.Service.History;
using ClaimsMS.Core.Service.Notification;
using ClaimsMS.Core.Service.User;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Resolutions;
using ClaimsMS.Infrastructure;
using ClaimsMS.Infrastructure.Database.Context.Mongo;
using ClaimsMS.Infrastructure.RabbitMQ.Connection;
using ClaimsMS.Infrastructure.RabbitMQ.Consumer;
using ClaimsMS.Infrastructure.Service.Notification;
using ClaimsMS.Infrastructure.Services.Auction;
using ClaimsMS.Infrastructure.Services.History;
using ClaimsMS.Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PaymentMS.Infrastructure.Services.User;
using ProductsMS.Infrastructure.RabbitMQ;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration)
        .AddInfrastructure(builder.Configuration)
        .AddApplication();

// Registrar el serializador de GUID
BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
// Registro de los perfiles de AutoMapper
var profileTypes = new[]
{
    typeof(ResolutionProfile),
     typeof(ClaimProfile),
     typeof(ClaimDeliveryProfile),
};

foreach (var profileType in profileTypes)
{
    builder.Services.AddAutoMapper(profileType);
}

builder.Services.AddSingleton<IApplicationDbContextMongo>(sp =>
{
    const string connectionString = "mongodb+srv://yadefreitas19:08092001@cluster0.owy2d.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
    var databaseName = "ClaimMs";
    return new ApplicationDbContextMongo(connectionString, databaseName);
});

builder.Services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();
builder.Services.AddSingleton<IConnectionRabbbitMQ, RabbitMQConnection>();


builder.Services.AddScoped(sp =>
{
    var dbContext = sp.GetRequiredService<IApplicationDbContextMongo>();
    return dbContext.Database.GetCollection<ResolutionEntity>("Resolutions"); // Nombre de la colección en MongoDB
});

builder.Services.AddScoped(sp =>
{
    var dbContext = sp.GetRequiredService<IApplicationDbContextMongo>();
    return dbContext.Database.GetCollection<ClaimEntity>("Claims"); // Nombre de la colección en MongoDB
});

builder.Services.AddScoped(sp =>
{
    var dbContext = sp.GetRequiredService<IApplicationDbContextMongo>();
    return dbContext.Database.GetCollection<ClaimDelivery>("ClaimDeliveries"); // Nombre de la colección en MongoDB
});

builder.Services.AddSingleton<IMongoCollection<GetResolutionDto>>(provider =>
{
    var mongoClient = new MongoClient("mongodb+srv://yadefreitas19:08092001@cluster0.owy2d.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
    var database = mongoClient.GetDatabase("ClaimMs");
    return database.GetCollection<GetResolutionDto>("Resolutions");
});

builder.Services.AddSingleton<IMongoCollection<GetClaimDto>>(provider =>
{
    var mongoClient = new MongoClient("mongodb+srv://yadefreitas19:08092001@cluster0.owy2d.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
    var database = mongoClient.GetDatabase("ClaimMs");
    return database.GetCollection<GetClaimDto>("Claims");
});

builder.Services.AddSingleton<IMongoCollection<GetClaimDeliveryDto>>(provider =>
{
    var mongoClient = new MongoClient("mongodb+srv://yadefreitas19:08092001@cluster0.owy2d.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
    var database = mongoClient.GetDatabase("ClaimMs");
    return database.GetCollection<GetClaimDeliveryDto>("ClaimDeliveries");
});



builder.Services.AddSingleton<IConnectionFactory>(provider =>
{
    return new ConnectionFactory
    {
        HostName = "localhost",
        Port = 5672,
        UserName = "guest",
        Password = "guest",
    };
});

builder.Services.AddSingleton<IConnectionRabbbitMQ>(provider =>
{
    var connectionFactory = provider.GetRequiredService<IConnectionFactory>();
    var rabbitMQConnection = new RabbitMQConnection(connectionFactory);
    rabbitMQConnection.InitializeAsync().Wait(); // ?? Ejecutar inicialización antes de inyectarlo
    return rabbitMQConnection;
});
builder.Services.AddSingleton(typeof(IEventBus<>), typeof(RabbitMQProducer<>));

builder.Services.AddSingleton<IEventBus<GetResolutionDto>>(provider =>
{
    var rabbitMQConnection = provider.GetRequiredService<IConnectionRabbbitMQ>();
    return new RabbitMQProducer<GetResolutionDto>(rabbitMQConnection);
});
builder.Services.AddSingleton<IEventBus<GetClaimDto>>(provider =>
{
    var rabbitMQConnection = provider.GetRequiredService<IConnectionRabbbitMQ>();
    return new RabbitMQProducer<GetClaimDto>(rabbitMQConnection);
});

builder.Services.AddSingleton<IEventBus<GetClaimDeliveryDto>>(provider =>
{
    var rabbitMQConnection = provider.GetRequiredService<IConnectionRabbbitMQ>();
    return new RabbitMQProducer<GetClaimDeliveryDto>(rabbitMQConnection);
});

builder.Services.AddSingleton<RabbitMQConsumer>(provider =>
{

    var rabbitMQConnection = provider.GetRequiredService<IConnectionRabbbitMQ>();
    var resoCollection = provider.GetRequiredService<IMongoCollection<GetResolutionDto>>();
    var claimCollection = provider.GetRequiredService<IMongoCollection<GetClaimDto>>();
    var claimDeliveryCollection = provider.GetRequiredService<IMongoCollection<GetClaimDeliveryDto>>();
    return new RabbitMQConsumer(rabbitMQConnection, resoCollection, claimCollection, claimDeliveryCollection);
});



builder.Services.AddHostedService<RabbitMQBackgroundService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen();


var _appSettings = new AppSettings();
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
_appSettings = appSettingsSection.Get<AppSettings>();
builder.Services.Configure<AppSettings>(appSettingsSection);

builder.Services.Configure<HttpClientUrl>(
    builder.Configuration.GetSection("HttpClientAddress"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddHttpClient<IAuctionService, AuctionService>();
builder.Services.AddHttpClient<INotificationService, NotificationService>();
builder.Services.AddHttpClient<IHistoryService, HistoryService>();




// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Connected!");

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();
app.Run();



