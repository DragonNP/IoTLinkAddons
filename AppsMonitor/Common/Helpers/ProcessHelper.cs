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
        public static bool SetProcessState(ref ProcessMonitor monitor)
        {
            Process[] processes = Process.GetProcessesByName(monitor.ProcessName);

            if (processes.Count() == 0)
            {
                if (monitor.State != ProcessState.NotRunning)
                {
                    monitor.State = ProcessState.NotRunning;
                    return true;
                }
            }
            else
            {
                if (monitor.State != ProcessState.Running)
                {
                    monitor.State = ProcessState.Running;
                    return true;
                }
            }

            return false;
        }
        public static void StartProcess(ProcessMonitor monitor)
        {
            if (monitor == null)
                return;

            RunInfo runInfo = new RunInfo
            {
                Application = monitor.PathToExe
            };

            PlatformHelper.Run(runInfo);

        }

        public static void KillProcess(ProcessMonitor monitor)
        {
            if (monitor == null)
                return;

            Process[] processes = Process.GetProcessesByName(monitor.ProcessName);

            if (processes.Count() != 0)
                processes[0].Kill();
        }
    }
}
