using SE.Core.DTO;
using SE.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.CommonServices
{
    public interface ICommonService
    {
        Task<IDataResult<DashboardDataDto>> GetDashboardDataAsync(DashboardFilterDto dashboardFilterDto);
    }
}
