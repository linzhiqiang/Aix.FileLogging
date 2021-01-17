using Microsoft.Extensions.Hosting;
using System;
using Aix.FileLogging;

namespace Aix.FileLoggingSample
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
               .ConfigureHostConfiguration(configurationBuilder =>
               {
               })
              .ConfigureAppConfiguration((hostBulderContext, configurationBuilder) =>
              {

              })
               .ConfigureLogging((hostBulderContext, loggingBuilder) =>
               {
                   loggingBuilder.AddAixFileLog();

               })
               .ConfigureServices(Startup.ConfigureServices);
        }
    }
}
