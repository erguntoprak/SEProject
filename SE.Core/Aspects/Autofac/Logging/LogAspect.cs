using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SE.Core.CrossCuttingConcerns.Logging;
using SE.Core.CrossCuttingConcerns.Logging.Serilog;
using SE.Core.Utilities.Interceptors;
using SE.Core.Utilities.IoC;
using SE.Core.Utilities.Messages;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Linq;

namespace SE.Core.Aspects.Autofac.Logging
{
    /// <summary>
    /// LogAspect
    /// </summary>
    public class LogAspect : MethodInterception
    {
        private readonly LoggerServiceBase _loggerServiceBase;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogAspect(Type loggerService)
        {

            if (loggerService.BaseType != typeof(LoggerServiceBase))
            {
                throw new ArgumentException(AspectMessages.WrongLoggerType);
            }

            _loggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(loggerService);
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }
        protected override void OnBefore(IInvocation invocation)
        {
            _loggerServiceBase?.Info(GetLogDetail(invocation));
        }

        private string GetLogDetail(IInvocation invocation)
        {
            var logParameters = new List<LogParameter>();
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                    Value = invocation.Arguments[i],
                    Type = invocation.Arguments[i].GetType().Name

                });
            }
            var logDetail = new LogDetail
            {
                FullName = invocation.TargetType.Name,
                MethodName = invocation.Method.Name,
                Parameters = logParameters,
                UserEmail = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value

            };
            return JsonConvert.SerializeObject(logDetail);
        }
    }
}
