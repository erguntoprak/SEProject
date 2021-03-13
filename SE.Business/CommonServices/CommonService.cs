using SE.Core.DTO;
using SE.Core.Utilities.Results;
using SE.Data;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SE.Business.Infrastructure.ConfigurationManager;
using Microsoft.Extensions.Options;
using SE.Business.EmailSenders;
using SE.Business.Constants;
using SE.Core.Utilities.Security.Http;

namespace SE.Business.CommonServices
{
    public class CommonService : ICommonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Configuration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IRequestContext _requestContext;



        public CommonService(IUnitOfWork unitOfWork, IOptions<Configuration> configuration, IEmailSender emailSender, IRequestContext requestContext)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration.Value;
            _emailSender = emailSender;
            _requestContext = requestContext;
        }
        public async Task<IDataResult<DashboardDataDto>> GetDashboardDataAsync()
        {
            DashboardDataDto dashboardDataDto = new DashboardDataDto();

            var educationList = await _unitOfWork.EducationRepository.Table.Where(d => d.UserId == _requestContext.UserId).ToListAsync();

            dashboardDataDto.EducationTotalCount = educationList.Count;

            var blogList = await _unitOfWork.BlogRepository.Table.Where(d => d.UserId == _requestContext.UserId).ToListAsync();
            dashboardDataDto.BlogTotalCount = blogList.Count;

            var educationIds = educationList.Select(d => d.Id);
            var contactFormList = await _unitOfWork.EducationContactFormRepository.Table.Where(d => educationIds.Contains(d.EducationId)).ToListAsync();
            dashboardDataDto.ContactFormCount = contactFormList.Count;

            return new SuccessDataResult<DashboardDataDto>(dashboardDataDto);
        }

        public async Task<IResult> SendContactFormAsync(ContactFormDto contactFormDto)
        {
            await _emailSender.SendAsync(_configuration.ContactEmail, "İletişim Formu!", EmailMessages.GetContactFormHtml(contactFormDto, _configuration.ApiUrl));
            return new SuccessResult(Messages.EmailSended);
        }
    }
}
