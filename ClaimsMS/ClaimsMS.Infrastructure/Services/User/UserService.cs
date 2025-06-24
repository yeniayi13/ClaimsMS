using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

using ClaimsMS.Core.Service.User;
using ClaimsMS.Common.Dtos.Others;
using ClaimsMS.Infrastructure;

namespace PaymentMS.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _httpClientUrl;
        public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<HttpClientUrl> httpClientUrl)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClientUrl = httpClientUrl.Value.ApiUrl;

            //* Configuracion del HttpClient
            var headerToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
            _httpClient.BaseAddress = new Uri("http://localhost:18084/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {headerToken}");
        }

        public async Task<GetUser> BidderExists(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"user/bidder/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error al obtener usuario: {response.StatusCode}");
                }

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                if (responseStream == null)
                {
                    throw new InvalidOperationException("El contenido de la respuesta es nulo.");
                }

                var user = await JsonSerializer.DeserializeAsync<GetUser>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (user == null)
                {
                    throw new InvalidOperationException("No se pudo deserializar el usuario.");
                }

                Console.WriteLine($"User ID: {user.UserId}, Name: {user.UserName}");

                return user;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error de solicitud HTTP: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine($"Operación inválida: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.Error.WriteLine($"Error de deserialización JSON: {ex.Message}");
                throw;
            }
            catch (System.Exception ex)
            {
                Console.Error.WriteLine($"Error inesperado: {ex.Message}");
                throw;
            }
        }

        public async Task<GetUser> AuctioneerExists(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"user/auctioneer/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error al obtener usuario: {response.StatusCode}");
                }

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                if (responseStream == null)
                {
                    throw new InvalidOperationException("El contenido de la respuesta es nulo.");
                }

                var user = await JsonSerializer.DeserializeAsync<GetUser>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (user == null)
                {
                    throw new InvalidOperationException("No se pudo deserializar el usuario.");
                }

                Console.WriteLine($"User ID: {user.UserId}, Name: {user.UserName}");

                return user;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error de solicitud HTTP: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine($"Operación inválida: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.Error.WriteLine($"Error de deserialización JSON: {ex.Message}");
                throw;
            }
            catch (System.Exception ex)
            {
                Console.Error.WriteLine($"Error inesperado: {ex.Message}");
                throw;
            }
        }

    }
}
