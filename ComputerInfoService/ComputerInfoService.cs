using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace ComputerInfoService
{
    public partial class ComputerInfoService : ServiceBase
    {
        private Timer timer;
        private EventLog eventLog;
        public ComputerInfoService()
        {
            this.ServiceName = "ComputerInfoService";

            eventLog = new EventLog();
            if (!EventLog.SourceExists("ComputerInfoSource"))
            {
                throw new InvalidOperationException("La fuente de eventos 'ComputerInfoSource' no existe. Por favor, créala manualmente antes de iniciar el servicio.");
            }
            eventLog.Source = "ComputerInfoSource";
            eventLog.Log = "ComputerInfoLog";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                eventLog.WriteEntry("Trying to start the service.");

                Task.Run(() =>
                {
                    timer = new Timer(300000);
                    timer.Elapsed += CollectComputerInfo;
                    timer.AutoReset = true;
                    timer.Enabled = true;

                    CollectComputerInfo(null, null);

                    eventLog.WriteEntry("Service started successfully.");
                });

                eventLog.WriteEntry("Service started correctly.");
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry($"ERROR a OnStart: {ex.ToString()}", EventLogEntryType.Error);
                throw;
            }
        }

        private void CollectComputerInfo(object sender, ElapsedEventArgs e)
        {
            try
            {
                string computerName = Environment.MachineName;
                string osInfo = $"{Environment.OSVersion} (64-bit: {Environment.Is64BitOperatingSystem})";

                string logMessage = $"Recopilado: Nombre={computerName}, SO={osInfo}";
                eventLog.WriteEntry(logMessage, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry($"Error: {ex.Message}", EventLogEntryType.Error);
            }
        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
