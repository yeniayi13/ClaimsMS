using ClaimsMS.Common.Dtos.Product.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Core.Service.History
{
    public interface IHistoryService
    {
        Task AddActivityHistoryAsync(GetHistoryDto history);
    }
}
