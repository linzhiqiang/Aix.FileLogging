using Aix.FileLogging.Model;
using Aix.FileLogging.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aix.FileLogging
{
    public class FileLoggerProcessor : LoggerProcessorBase
    {

        private FileLogOptions _options;
        private StreamWriter _streamWriter;
        private string _currentFileName;
        private string _logDIr;

        public FileLoggerProcessor(FileLogOptions options)
        {
            _options = options;
            _logDIr = options.LogDIr;

            Init();
        }

        private string GetTodayPrefix()
        {
            return "log-" + DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void Init()
        {
            if (!Directory.Exists(_logDIr))
            {
                Directory.CreateDirectory(_logDIr);
            }

            var todayFileNamePrefix = GetTodayPrefix();
            var files = Directory.GetFiles(_logDIr, $"{todayFileNamePrefix}*.log");

            if (files == null || files.Length == 0)
            {
                string path = Path.Combine(_logDIr, $"{todayFileNamePrefix}.log");
                CreateStreamWriter(path);
            }
            else
            {
                var targetFile = files.OrderByDescending(x => x).First();
                CreateStreamWriter(targetFile);
            }
        }

        private void CreateStreamWriter(string filePath)
        {
            var fileStream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            //var writeBufferedStream = new BufferedStream(fileStream);
            _streamWriter = new StreamWriter(fileStream);
            _currentFileName = Path.GetFileName(fileStream.Name);
        }

        private void Check()
        {
            if (!IsSameDay())
            {
                CloseStreamWriter();

                  var todayFileNamePrefix = GetTodayPrefix();
                string path = Path.Combine(_logDIr, $"{todayFileNamePrefix}.log");
                CreateStreamWriter(path);
                return;
            }
            if (_streamWriter.BaseStream.Position >= _options.FileMaxSize)
            {
                _streamWriter.Flush();
                _streamWriter.Close();

                var sequene = GetTodayNextSequene();
                var todayFileNamePrefix = GetTodayPrefix();
                string path = Path.Combine(_logDIr, $"{todayFileNamePrefix}_{sequene}.log");
                CreateStreamWriter(path);
                return;
            }
        }

        protected override Task WriteMessage(LogMessageEntry message)
        {
            Check();
            // Console.WriteLine(message.Message);
            // await _streamWriter.WriteLineAsync(message.Message);
            // await _streamWriter.FlushAsync();
            _streamWriter.WriteLine(message.Message);
            _streamWriter.Flush();
            return Task.CompletedTask;
        }

        protected override void Close()
        {
            CloseStreamWriter();
        }

        protected  void CloseStreamWriter()
        {
            _streamWriter.Flush();
            _streamWriter.Close();
        }
        private bool IsSameDay()
        {
            return _currentFileName.StartsWith(GetTodayPrefix());
        }

        private int GetTodayNextSequene()
        {
            int result = 0;
            int index = _currentFileName.IndexOf("_");
            if (index >= 0)
            {
                var sequeneStr = Path.GetFileNameWithoutExtension(_currentFileName).Substring(index + 1);
                result = NumberUtils.ToInt(sequeneStr);
            }
            return result + 1;
        }

    }
}
