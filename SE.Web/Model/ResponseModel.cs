using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            ErrorMessage = new List<string>();
        }
        public List<string> ErrorMessage { get; set; }
        public object Data { get; set; }
    }
}
