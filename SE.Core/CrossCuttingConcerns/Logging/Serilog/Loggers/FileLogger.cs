using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using SE.Core.Utilities.IoC;
using SE.Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Serilog;

namespace SE.Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class FileLogger : LoggerServiceBase
    {
        public FileLogger()
        {
            IConfiguration configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:FileLogConfiguration")
                .Get<FileLogConfiguration>() ?? throw new Exception(Utilities.Messages.SerilogMessages.NullOptionsMessage);

            string logFilePath = string.Format("{0}{1}", Directory.GetCurrentDirectory() + logConfig.FolderPath, ".txt");

            _logger = new LoggerConfiguration()
                        .WriteTo.File(logFilePath,
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: null,
                        fileSizeLimitBytes: 5000000,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
                        .CreateLogger();
        }
    }
}
