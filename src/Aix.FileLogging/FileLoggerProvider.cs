using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Aix.FileLogging
{
    public class MyFileLoggerProvider : Microsoft.Extensions.Logging.ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, FileLogger> _loggers = new ConcurrentDictionary<string, FileLogger>();

        private readonly FileLoggerProcessor _messageQueue;
        private FileLogOptions _options;

        public MyFileLoggerProvider(FileLogOptions options)
        {
            _options = options;
            _messageQueue = new FileLoggerProcessor(_options);
        }
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        private FileLogger CreateLoggerImplementation(string categoryName)
        {
            return new FileLogger(categoryName, _options, _messageQueue);
        }

        public void Dispose()
        {
            _messageQueue.Dispose();
        }
    }
}
