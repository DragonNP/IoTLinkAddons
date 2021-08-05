using AppsMonitor.Common.Configs;
using AppsMonitor.Common.Processes;
using IOTLinkAPI.Helpers;
using IOTLinkAPI.Platform;
using System.Diagnostics;
using System.Linq;

namespace AppsMonitor.Common.Helpers
{
    static class ProcessHelper
    {
        public static Process[] GetProcesses(string process_name)
        {
            Process[] processes = Process.GetProcessesByName(process_name);
            return processes;
        }

        public static bool UpdateState(ref ProcessMonitor monitor)
        {
            bool isUpdated = false;

            if (monitor == null || monitor.Processes == null)
                return isUpdated;

            if (monitor.Processes.Count() == 0)
                monitor.Processes = GetProcesses(monitor.ProcessName);
            if (monitor.Processes.Count() == 0 && monitor.State == ProcessState.Running)
            {
                monitor.State = ProcessState.NotRunning;
                return true;
            }

            try
            {
                var process = monitor.Processes[0];
                if (process.HasExited && monitor.State == ProcessState.Running)
                {
                    monitor.State = ProcessState.NotRunning;
                    monitor.ClearProcesses();
                    return true;
                }
                else if (!process.HasExited && monitor.State == ProcessState.NotRunning)
                {
                    monitor.State = ProcessState.Running;
                    return true;
                }
            }
            catch {}

            return isUpdated;
        }

        public static void KillProcesses(ProcessMonitor monitor)
        {
            if (monitor == null)
                return;
            if (monitor.Processes.Count() == 0)
                monitor.Processes = GetProcesses(monitor.ProcessName);

            foreach (var process in monitor.Processes)
                process.Kill();

            monitor.ClearProcesses();
        }

        public static void StartProcess(ProcessMonitor monitor)
        {
            if (monitor == null || string.IsNullOrEmpty(monitor.PathToExe))
                return;

            LoggerHelper.Info("ProcessHelper.StartProcess({0}): start process path:{1}", monitor.DisplayName, monitor.PathToExe);
            RunInfo runInfo = new RunInfo
            {
                Application = monitor.PathToExe
            };
            PlatformHelper.Run(runInfo);
            
            monitor.Processes = GetProcesses(monitor.ProcessName);
        }
    }
}
