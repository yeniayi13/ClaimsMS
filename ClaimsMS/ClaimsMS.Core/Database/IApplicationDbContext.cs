using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClaimsMS.Core.Database;
using ClaimsMS.Domain.Entities.Claim;
using ClaimsMS.Domain.Entities.Resolution;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Resolutions;

namespace ClaimsMS.Core.Database
{
    public interface IApplicationDbContext
    {
        DbContext DbContext { get; }
        DbSet<ClaimEntity> Claims { get; set; }
        DbSet<ResolutionEntity> Resolutions { get; set; }

        DbSet<ClaimDelivery> ClaimDeliveries { get; set; }


        IDbContextTransactionProxy BeginTransaction();

        void ChangeEntityState<TEntity>(TEntity entity, EntityState state);

        Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default);
    }
}