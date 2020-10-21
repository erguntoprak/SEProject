using NETCore.MailKit.Core;
using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.CommonServices
{
    public class CommonService : ICommonService
    {
        private readonly IEmailService _emailSender;

        public CommonService(IEmailService emailSender)
        {
            _emailSender = emailSender;
        }
        public bool SendContactForm(ContactFormDto contactFormDto)
        {
            return false;
        }
    }
}
