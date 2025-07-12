using ClaimsMS.Domain.Entities.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Domain.Entities.Claims.ValueObject;

namespace ClaimsMS.Infrastructure.Database.Configuration.Postgres
{
    public class ClaimDeliveryConfiguration : IEntityTypeConfiguration<ClaimDelivery>
    {
        public void Configure(EntityTypeBuilder<ClaimDelivery> builder)
        {


            builder.ToTable("ClaimDeliveries");
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
                  .HasConversion(claimReason => claimReason.Value, value => ClaimReason.Create(value)!)
                  .IsRequired();
            builder.Property(s => s.ClaimDeliveryId)
                  .HasConversion(claimDeliveryId => claimDeliveryId.Value, value => ClaimDeliveryId.Create(value)!)
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
