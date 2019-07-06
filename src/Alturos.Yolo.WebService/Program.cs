using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using Topshelf;

namespace Alturos.Yolo.WebService
{
    class Program
    {
        static int Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var log4netConfig = new FileInfo("log4net.config");
            if (!log4netConfig.Exists)
            {
                Console.WriteLine("[Warning] Cannot found a log4net.config");
            }

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.ConfigureAndWatch(logRepository, log4netConfig);

            var exitCode = HostFactory.Run(x =>
            {
                x.Service<Controller>();
                x.RunAsLocalSystem();
                x.SetDescription("Alturos Yolo WebService");
                x.SetDisplayName("Alturos.Yolo.WebService");
                x.SetServiceName("Alturos.Yolo.WebService");
            });

            return (int)exitCode;
        }
    }
}
