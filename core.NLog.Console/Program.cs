using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;


namespace core.NLog.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var servicesProvider = Runner.BuildDi(config);
                using (servicesProvider as IDisposable)
                {
                    var runner = servicesProvider.GetRequiredService<Runner>();
                    runner.DoAction("Action1");

                    System.Console.WriteLine("Press ANY key to exit");
                    System.Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
    }
}
