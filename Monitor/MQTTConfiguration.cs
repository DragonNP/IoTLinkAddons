namespace Monitor
{
    class MQTTConfiguration
    {
        public string CpuClocksTopic { get; }
        public string CpuTemperaturesTopic { get; }
        public string CpuLoadTopic { get; }
        public string CpuPowersTopic { get; }

        public MQTTConfiguration()
        {
            CpuClocksTopic = "stats/cpu/clocks/";
            CpuTemperaturesTopic = "stats/cpu/temperatures/";
            CpuLoadTopic = "stats/cpu/load/";
            CpuPowersTopic = "stats/cpu/powers/";
        }
    }
}
