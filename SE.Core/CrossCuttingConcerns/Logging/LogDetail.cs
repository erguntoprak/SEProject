using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.CrossCuttingConcerns.Logging
{
    public class LogDetail
    {
        public string FullName { get; set; }
        public string MethodName { get; set; }
        public List<LogParameter> Parameters { get; set; }
        public string UserEmail { get; set; }
    }
}
