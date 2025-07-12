
using ClaimsMS.Common.Dtos.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClaimsMS.Core.Service.User
{
    public interface IUserService
    {
        Task<GetUser> BidderExists(Guid userId);
        Task<GetUser> AuctioneerExists(Guid userId);

    }
}
