using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Core.Service.Auction
{
    public interface IAuctionService
    {
        Task<bool> AuctionExists(Guid auctionId);
    }
}
