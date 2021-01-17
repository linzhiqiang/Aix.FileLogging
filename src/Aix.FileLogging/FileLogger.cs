using Aix.FileLogging.Foundation;
using Aix.FileLogging.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aix.FileLogging
{
    public class FileLogger : Microsoft.Extensions.Logging.ILogger
    {

        public string Name { get; }
        protected FileLogOptions _options;


        private FileLoggerProcessor _messageQueue;

        public FileLogger(string name, FileLogOptions options, FileLoggerProcessor messageQueue)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            _options = options;
            this.Name = name;
            _messageQueue = messageQueue;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            StringBuilder sb = new StringBuilder();

            //日志级别上层已经根据级别过滤了，到了这里就是满足日志级别了，只管记录即可
            //var msg = formatter(state, exception); //内部实现 是state.ToString()

            var msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {Name} [{logLevel}] {formatter(state, exception)}";
            sb.Append(msg);
            if (exception != null)
            {
                sb.AppendLine();
                sb.Append(exception.ToString());
            }

            _messageQueue.EnqueueMessage(new LogMessageEntry { Message = sb.ToString() });


        }
    }
}
