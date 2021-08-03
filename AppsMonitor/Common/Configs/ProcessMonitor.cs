using AppsMonitor.Common.Helpers;
using AppsMonitor.Common.Processes;
using IOTLinkAPI.Configs;

namespace AppsMonitor.Common.Configs
{
    public class ProcessMonitor
    {
        public string ProcessName { get; set; }
        public string PathToExe { get; set; }
        public string DisplayName { get; set; }
        public ProcessState State { get; set; }
        public string UserNameRunning { get; set; }

        public static ProcessMonitor FromConfiguration(Configuration configuration)
        {
            var monitor = new ProcessMonitor
            {
                ProcessName = configuration.GetValue("process_name", ""),
                PathToExe = configuration.GetValue("path_to_exe", ""),
                DisplayName = configuration.GetValue("display_name", ""),
                UserNameRunning = configuration.GetValue("username_running", ""),
            };

            ProcessHelper.SetProcessState(ref monitor);

            return monitor;
        }
    }
}
