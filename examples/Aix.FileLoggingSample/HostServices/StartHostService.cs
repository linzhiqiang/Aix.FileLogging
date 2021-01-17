using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aix.FileLoggingSample.HostServices
{
    public class StartHostService : IHostedService
    {
        ILogger<StartHostService> _logger;

        public StartHostService(ILogger<StartHostService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Task.Run(()=> {
                for (int i = 0; i < 1000; i++)
                {
                    _logger.LogInformation("message"+i);
                }
            });
           

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
