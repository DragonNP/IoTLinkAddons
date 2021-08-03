using IOTLinkAPI.Platform;

namespace AppsMonitor.Common.Processes
{
    public class ProcessInformation : ProcessInfo
    {
        public ProcessState Status { get; set; }
        public string Name { get; set; }
    }

}
