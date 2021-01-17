using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aix.FileLogging
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddAixFileLog(this ILoggerFactory factory, Action<FileLogOptions> setup = null)
        {
            FileLogOptions options = new FileLogOptions();

            if (setup != null)
            {
                setup(options);
            }
            Valid(options);
            factory.AddProvider(new MyFileLoggerProvider(options));

            return factory;
        }

        public static ILoggingBuilder AddAixFileLog(this ILoggingBuilder factory, Action<FileLogOptions> setup = null)
        {
            FileLogOptions options = new FileLogOptions();

            if (setup != null)
            {
                setup(options);
            }
            Valid(options);
            factory.AddProvider(new MyFileLoggerProvider(options));
            if (options.logLevel.HasValue)
            {
                factory.SetMinimumLevel(options.logLevel.Value);
            }

            return factory;
        }

        private static void Valid(FileLogOptions options)
        {
            if (string.IsNullOrEmpty(options.LogDIr))
            {
                options.LogDIr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            }

            //1024 * 1024 =1M
            if (options.FileMaxSize < 1024 * 1024)
            {
                options.FileMaxSize = 1024 * 1024;
            }
        }
    }
}
