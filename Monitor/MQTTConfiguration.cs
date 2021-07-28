namespace Monitor
{
    class MQTTConfiguration
    {
        private readonly string _cpuClockCore, _cpuClockBusSpeed;
        private readonly string _cpuTemperatureCore, _cpuTemperatureMax, _cpuTemperatureAverage, _cpuTemperaturePackage;
        private readonly string _cpuLoadCore, _cpuLoadTotal;
        private readonly string _cpuPowerPackage, _cpuPowerCores, _cpuPowerGraphics, _cpuPowerMemory, _cpuPowerAll;

        public MQTTConfiguration()
        {
            _cpuClockCore = "stats/cpu/clocks/core{0}";
            _cpuClockBusSpeed = "stats/cpu/clocks/bus-speed";

            _cpuTemperatureCore = "stats/cpu/temperatures/core{0}";
            _cpuTemperatureMax = "stats/cpu/temperatures/max";
            _cpuTemperatureAverage = "stats/cpu/temperatures/average";
            _cpuTemperaturePackage = "stats/cpu/temperatures/package";

            _cpuLoadCore = "stats/cpu/load/core{0}";
            _cpuLoadTotal = "stats/cpu/load/total";

            _cpuPowerPackage = "stats/cpu/powers/package";
            _cpuPowerCores = "stats/cpu/powers/cores";
            _cpuPowerGraphics = "stats/cpu/powers/graphics";
            _cpuPowerMemory = "stats/cpu/powers/memory";
            _cpuPowerAll = "stats/cpu/powers/all";
        }

        public string GetClockTopic(string type)
        {
            switch (type)
            {
                case "core":
                    return _cpuClockCore;
                case "bus-speed":
                    return _cpuClockBusSpeed;
                default:
                    return "";
            }
        }

        public string GetTempTopic(string type)
        {
            switch (type)
            {
                case "core":
                    return _cpuTemperatureCore;
                case "max":
                    return _cpuTemperatureMax;
                case "average":
                    return _cpuTemperatureAverage;
                case "package":
                    return _cpuTemperaturePackage;
                default:
                    return "";
            }
        }

        public string GetLoadTopic(string type)
        {
            switch (type)
            {
                case "core":
                    return _cpuLoadCore;
                case "total":
                    return _cpuLoadTotal;
                default:
                    return "";
            }
        }

        public string GetPowerTopic(string type)
        {
            switch (type)
            {
                case "package":
                    return _cpuPowerPackage;
                case "cores":
                    return _cpuPowerCores;
                case "graphics":
                    return _cpuPowerGraphics;
                case "memory":
                    return _cpuPowerMemory;
                case "all":
                    return _cpuPowerAll;
                default:
                    return "";
            }
        }
    }
}
