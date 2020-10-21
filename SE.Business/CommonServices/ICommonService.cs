using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.CommonServices
{
    interface ICommonService
    {
        bool SendContactForm(ContactFormDto contactFormDto);
    }
}
