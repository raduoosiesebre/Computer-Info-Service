using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace ComputerInfoService
{
    internal static class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            // console mode for debugging
            var service = new ComputerInfoService();
            Console.writeLine("Starting service in debug mode...");
            service.OnDebug();
            Console.writeLine("Press enter to exit...");
            Console.ReadLine();
            service.Stop();
#else
            // real mode
            ServiceBase[] servicesToRun = { new ComputerInfoService() };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}
