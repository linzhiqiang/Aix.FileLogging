using Aix.FileLogging.Model;
using Aix.FileLogging.Utils;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Aix.FileLogging
{
    public abstract class LoggerProcessorBase : IDisposable
    {
        private const int _maxQueuedMessages = 10000000; //1000万

        private readonly BlockingCollection<LogMessageEntry> _messageQueue = new BlockingCollection<LogMessageEntry>(_maxQueuedMessages);
        private readonly Thread _outputThread;

        public LoggerProcessorBase()
        {
            _outputThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Name = "file logger queue processing thread"
            };
            _outputThread.Start();
        }

        public void EnqueueMessage(LogMessageEntry message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    _messageQueue.Add(message);
                    return;
                }
                catch (InvalidOperationException) { }
            }

            // Adding is completed so just log the message
            WriteMessage(message);
        }


        private void ProcessLogQueue()
        {
            try
            {
                foreach (var message in _messageQueue.GetConsumingEnumerable())
                {
                    WriteMessage(message);
                }

            }
            catch
            {
                try
                {
                    _messageQueue.CompleteAdding();
                }
                catch { }
            }
        }

        protected abstract Task WriteMessage(LogMessageEntry message);

        protected virtual void Close() { }

        public void Dispose()
        {
            With.NoException(() => {
                _messageQueue.CompleteAdding();
                Close();
            });

        }
    }
}
