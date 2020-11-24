using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Common
{
    public class UploadImageModel
    {
        public IFormCollection Images { get; set; }
    }
}
