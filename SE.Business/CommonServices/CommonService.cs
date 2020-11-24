using NETCore.MailKit.Core;
using SE.Core.DTO;
using SE.Core.Utilities.Results;
using SE.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SE.Business.CommonServices
{
    public class CommonService : ICommonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IDataResult<DashboardDataDto>> GetDashboardDataAsync(DashboardFilterDto dashboardFilterDto)
        {
            DashboardDataDto dashboardDataDto = new DashboardDataDto();

            var educationList = await _unitOfWork.EducationRepository.Table.Where(d => d.UserId == dashboardFilterDto.UserId).ToListAsync();

            dashboardDataDto.EducationTotalCount = educationList.Count;

            dashboardDataDto.BlogTotalCount = (await _unitOfWork.BlogRepository.Table.Where(d => d.UserId == d.UserId).ToListAsync()).Count;

            var educationIds = educationList.Select(d => d.Id);
            dashboardDataDto.ContactFormCount = (await _unitOfWork.EducationContactFormRepository.Table.Where(d=> educationIds.Contains(d.EducationId)).ToListAsync()).Count;

            return new SuccessDataResult<DashboardDataDto>(dashboardDataDto);
        }
    }
}
