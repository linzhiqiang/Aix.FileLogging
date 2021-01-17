using Aix.FileLoggingSample.HostServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aix.FileLoggingSample
{
    public class Startup
    {
        internal static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            //入口服务
            services.AddHostedService<StartHostService>();
        }
    }
}
