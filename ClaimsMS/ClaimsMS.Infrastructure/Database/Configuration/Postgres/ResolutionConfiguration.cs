using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Domain.Entities.Resolution.ValueObject;
using ClaimsMS.Domain.Entities.Resolutions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ProductsMS.Infrastructure.Database.Configuration.Postgres
{

    [ExcludeFromCodeCoverage]
    public class ResolutionConfiguration : IEntityTypeConfiguration<ResolutionEntity>
    {
                public void Configure(EntityTypeBuilder<ResolutionEntity> builder)
                {


                        builder.ToTable("Resolutions");
                        builder.HasKey(s => s.ResolutionId);
                        builder.Property(s => s.ResolutionId)
                                .HasConversion(resolutionId => resolutionId.Value, value => ResolutionId.Create(value)!)
                                .IsRequired();
                        builder.Property(s => s.ResolutionDescription)
                                .HasConversion(resolutionDescription => resolutionDescription.Value, value => ResolutionDescription.Create(value)!)
                                .IsRequired();
                        builder.Property(s => s.ClaimId)
                                            .HasConversion(claimId => claimId.Value, value => ClaimId.Create(value)!)
                                            .IsRequired(false);
                             builder.Property(s => s.ClaimDeliveryId)
                                           .HasConversion(claimId => claimId.Value, value => ClaimId.Create(value)!)
                                           .IsRequired(false);
                             builder
                          .HasOne(r => r.Claim)
                          .WithOne(c => c.Resolution)
                          .HasForeignKey<ResolutionEntity>(r => r.ClaimId)
                          .IsRequired(false)
                          .OnDelete(DeleteBehavior.Cascade);

                        builder
                            .HasOne(r => r.ClaimDelivery)
                            .WithOne(c => c.Resolution)
                            .HasForeignKey<ResolutionEntity>(r => r.ClaimDeliveryId)
                            .IsRequired(false)
                            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}