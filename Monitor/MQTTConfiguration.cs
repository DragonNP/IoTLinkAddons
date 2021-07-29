namespace Monitor
{
    class MQTTConfiguration
    {
        public string CpuClocksTopic { get; }
        public string CpuTemperaturesTopic { get; }
        public string CpuPowersTopic { get; }

        public MQTTConfiguration()
        {
            CpuClocksTopic = "stats/cpu/clocks/";
            CpuTemperaturesTopic = "stats/cpu/temperatures/";
            CpuPowersTopic = "stats/cpu/powers/";
        }
    }
}
