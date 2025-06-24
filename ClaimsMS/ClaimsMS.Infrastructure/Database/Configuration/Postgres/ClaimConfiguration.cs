
using ClaimsMS.Domain.Entities.Claim;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Claims.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace ProductsMS.Infrastructure.Database.Configuration.Postgres
{

    [ExcludeFromCodeCoverage]
    public class ClaimConfiguration : IEntityTypeConfiguration<ClaimEntity>
        {
                public void Configure(EntityTypeBuilder<ClaimEntity> builder)
                {


                        builder.ToTable("Claims");
                        builder.HasKey(s => s.ClaimId);
                        builder.Property(s => s.ClaimId)
                                .HasConversion(claimId => claimId.Value, value => ClaimId.Create(value)!)
                                .IsRequired();

                        builder.Property(s => s.ClaimDescription)
                                .HasConversion(claimDescription => claimDescription.Value, value => ClaimDescription.Create(value)!)
                                .IsRequired();

                        builder.Property(s => s.ClaimEvidence)
                                .HasConversion(claimEvidence => claimEvidence.Base64Data, value => ClaimEvidence.FromBase64(value)!)
                                .IsRequired(false);
                        builder.Property(s => s.ClaimReason)
                              .HasConversion(claimReason => claimReason.Value  , value => ClaimReason.Create(value)!)
                              .IsRequired();
                        builder.Property(s => s.ClaimAuctionId)
                              .HasConversion(claimAuctionId => claimAuctionId.Value, value => ClaimAuctionId.Create(value)!)
                              .IsRequired();
                        builder.Property(s => s.ClaimUserId)
                                 .HasConversion(claimUserId => claimUserId.Value, value => ClaimUserId.Create(value)!)
                                 .IsRequired();
                        builder.Property(s => s.StatusClaim)
                                .HasConversion<string>()
                                .IsRequired();
                  
           

                }
        }
}