using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aix.FileLogging
{
    public class FileLogOptions
    {
        public FileLogOptions()
        {

        }
        public string LogDIr { get; set; }

        public LogLevel? logLevel { get; set; }

        /// <summary>
        ///    //1024 * 1024 =1M=1048576  1024 * 1024*1024=1G=1073741824
        /// </summary>
        public long FileMaxSize { get; set; } = 1073741824;
    }
}
