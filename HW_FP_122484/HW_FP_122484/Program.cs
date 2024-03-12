using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HW_FP_122484
{
    public class Program
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            LoadLog4netConfig();

            _log.Info("------程式啟動開始------");

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
          .ConfigureAppConfiguration((hostingContext, config) =>
          {
              config.SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile($"settings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"settings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
          })
          .UseStartup<Startup>();


        private static void LoadLog4netConfig()
        {
            var repository = LogManager.CreateRepository(
                    Assembly.GetEntryAssembly(),
                    typeof(log4net.Repository.Hierarchy.Hierarchy)
                );
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

    }
}