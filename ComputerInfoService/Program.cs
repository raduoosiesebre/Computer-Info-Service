using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace ComputerInfoService
{
    internal static class Program
    {
        static void Main(string[] args)
        {

            if (args.Length > 0 && args[0].Equals("console", StringComparison.OrdinalIgnoreCase))
            {
                // console mode to show the data of the computer
                string computerName = Environment.MachineName;
                string osInfo = $"{Environment.OSVersion} (64-bit: {Environment.Is64BitOperatingSystem})";
                Console.WriteLine($"Computer Name: {computerName}");
                Console.WriteLine($"OS Info: {osInfo}");
                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
                return;
            }
#if DEBUG
            // console mode for debugging
            var service = new ComputerInfoService();
            Console.WriteLine("Starting service in debug mode...");
            service.OnDebug();
            Console.WriteLine("Press enter to exit...");
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
