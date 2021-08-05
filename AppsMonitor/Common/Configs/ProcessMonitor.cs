using AppsMonitor.Common.Helpers;
using AppsMonitor.Common.Processes;
using IOTLinkAPI.Configs;
using System.Diagnostics;

namespace AppsMonitor.Common.Configs
{
    public class ProcessMonitor
    {
        public string ProcessName { get; set; }
        public string PathToExe { get; set; }
        public string DisplayName { get; set; }
        public ProcessState State { get; set; }
        public Process[] Processes { get; set; }
        public void ClearProcesses() => Processes = new Process[0];

        public static ProcessMonitor FromConfiguration(Configuration configuration)
        {
            var monitor = new ProcessMonitor
            {
                ProcessName = configuration.GetValue("process_name", ""),
                PathToExe = configuration.GetValue("path_to_exe", ""),
                DisplayName = configuration.GetValue("display_name", ""),
            };

            monitor.Processes = ProcessHelper.GetProcesses(monitor.ProcessName);
            ProcessHelper.UpdateState(ref monitor);

            return monitor;
        }
    }
}
