using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.EmailSenders
{
    public interface IEmailSender
    {
        public Task SendAsync(string mailTo, string subject, string message, bool isHtml = false);
    }
}
