using System;
using System.Linq;
using System.Timers;
using IOTLinkAPI.Addons;
using IOTLinkAPI.Helpers;
using IOTLinkAPI.Platform.HomeAssistant;
using LibreHardwareMonitor.Hardware;
using Timer = System.Timers.Timer;

namespace Monitor
{
    public class MonitorService : ServiceAddon
    {
        private Timer _monitorTimer;

        private string _tempGPUTopic;

        private string _cpuClockCore, _cpuClockBusSpeed;
        private string _cpuTemperatureCore, _cpuTemperatureMax, _cpuTemperatureAverage, _cpuTemperaturePackage;
        private string _cpuLoadCore, _cpuLoadTotal;
        private string _cpuPowerPackage, _cpuPowerCores, _cpuPowerGraphics, _cpuPowerMemory, _cpuPowerAll;
        private bool _isCPUName, _isCPUClocks, _isCPUTemperatures, _isCPULoad, _isCPUPowers;

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);

            _isCPUName = true;

            InitCPUClocks();
            InitCPUTemperatures();
            InitCPULoad();
            InitCPUPowers();

            //GetSensors();

            _tempGPUTopic = "stats/gpu/temperature";

            GetManager().PublishDiscoveryMessage(this, _tempGPUTopic, "GPU", DiscoveryOptions.Temperature());

            if (_isCPUName)
            {
                GetManager().PublishDiscoveryMessage(this, "stats/cpu/name", "CPU", new HassDiscoveryOptions
                {
                    Id = "Name",
                    Name = "Name",
                    Component = HomeAssistantComponent.Sensor,
                    Icon = "mdi:format-color-text"
                });

                var cpuName = CPU.Name();
                LoggerHelper.Info($"Sending cpu name: {cpuName}");
                GetManager().PublishMessage(this, "stats/cpu/name", cpuName.ToString());
            }

            _monitorTimer = new Timer();
            _monitorTimer.Interval = 10000;
            _monitorTimer.Elapsed += TimerElapsed;
            _monitorTimer.Start();
        }

        public void InitCPUClocks()
        {
            _isCPUClocks = true;

            _cpuClockCore = "stats/cpu/clocks/core{0}";
            _cpuClockBusSpeed = "stats/cpu/clocks/bus-speed";

            for (int core_num = 1; core_num < 5; core_num++)
                GetManager().PublishDiscoveryMessage(this, string.Format(_cpuClockCore, core_num), "CPU", DiscoveryOptions.Clock());
            GetManager().PublishDiscoveryMessage(this, _cpuClockBusSpeed, "CPU", DiscoveryOptions.Clock());
        }

        public void InitCPUTemperatures()
        {
            _isCPUTemperatures = true;

            _cpuTemperatureCore = "stats/cpu/temperatures/core{0}";
            _cpuTemperatureMax = "stats/cpu/temperatures/max";
            _cpuTemperatureAverage = "stats/cpu/temperatures/average";
            _cpuTemperaturePackage = "stats/cpu/temperatures/package";

            for (int core_num = 1; core_num < 5; core_num++)
                GetManager().PublishDiscoveryMessage(this, string.Format(_cpuTemperatureCore, core_num), "CPU", DiscoveryOptions.Temperature());
            GetManager().PublishDiscoveryMessage(this, _cpuTemperatureMax, "CPU", DiscoveryOptions.Temperature());
            GetManager().PublishDiscoveryMessage(this, _cpuTemperatureAverage, "CPU", DiscoveryOptions.Temperature());
            GetManager().PublishDiscoveryMessage(this, _cpuTemperaturePackage, "CPU", DiscoveryOptions.Temperature());
        }

        public void InitCPULoad()
        {
            _isCPULoad = true;

            _cpuLoadCore = "stats/cpu/load/core{0}";
            _cpuLoadTotal = "stats/cpu/load/total";

            for (int core_num = 1; core_num < 5; core_num++)
                GetManager().PublishDiscoveryMessage(this, string.Format(_cpuLoadCore, core_num), "CPU", DiscoveryOptions.Load());
            GetManager().PublishDiscoveryMessage(this, _cpuLoadTotal, "CPU", DiscoveryOptions.Load());
        }

        public void InitCPUPowers()
        {
            _isCPUPowers = true;

            _cpuPowerPackage = "stats/cpu/powers/package";
            _cpuPowerCores = "stats/cpu/powers/cores";
            _cpuPowerGraphics = "stats/cpu/powers/graphics";
            _cpuPowerMemory = "stats/cpu/powers/memory";
            _cpuPowerAll = "stats/cpu/powers/all";

            GetManager().PublishDiscoveryMessage(this, _cpuPowerPackage, "CPU", DiscoveryOptions.Power());
            GetManager().PublishDiscoveryMessage(this, _cpuPowerCores, "CPU", DiscoveryOptions.Power());
            GetManager().PublishDiscoveryMessage(this, _cpuPowerGraphics, "CPU", DiscoveryOptions.Power());
            GetManager().PublishDiscoveryMessage(this, _cpuPowerMemory, "CPU", DiscoveryOptions.Power());
            GetManager().PublishDiscoveryMessage(this, _cpuPowerAll, "CPU", DiscoveryOptions.Power());
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // CPU Clocks
            try
            {
                if (!_isCPUClocks)
                    return;

                LoggerHelper.Info($"Sending CPU clocks");

                for (int core_num = 1; core_num < 5; core_num++)
                {
                    var cpuClockCore = CPU.Clocks.GetCore(core_num);
                    GetManager().PublishMessage(this, string.Format(_cpuClockCore, core_num), cpuClockCore.ToString());
                }

                var cpuBusSpeed = CPU.Clocks.GetBusSpeed();
                GetManager().PublishMessage(this, _cpuClockBusSpeed, cpuBusSpeed.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send cpu clocks " + exception);
            }

            // CPU Temperatures
            try
            {
                if (!_isCPUTemperatures)
                    return;

                LoggerHelper.Info($"Sending CPU temperature");

                for (int core_num = 1; core_num < 5; core_num++)
                {
                    var cpuTempCore = CPU.Temperatures.GetCore(core_num);
                    GetManager().PublishMessage(this, string.Format(_cpuTemperatureCore, core_num), cpuTempCore.ToString());
                }

                var cpuTempPackage = CPU.Temperatures.GetPackage();
                var cpuTempMax = CPU.Temperatures.GetMax();
                var cpuTempAverage = CPU.Temperatures.GetAverage();

                GetManager().PublishMessage(this, _cpuTemperaturePackage, cpuTempPackage.ToString());
                GetManager().PublishMessage(this, _cpuTemperatureMax, cpuTempMax.ToString());
                GetManager().PublishMessage(this, _cpuTemperatureAverage, cpuTempAverage.ToString());


            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send cpu temperatures " + exception);
            }

            // CPU Load
            try
            {
                if (!_isCPULoad)
                    return;

                LoggerHelper.Info($"Sending CPU load");

                for (int core_num = 1; core_num < 5; core_num++)
                {
                    var cpuLoadCore = CPU.Load.GetCore(core_num);
                    GetManager().PublishMessage(this, string.Format(_cpuLoadCore, core_num), cpuLoadCore.ToString());
                }

                var cpuLoadTotal = CPU.Load.GetTotal();
                GetManager().PublishMessage(this, _cpuLoadTotal, cpuLoadTotal.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send cpu load " + exception);
            }

            // CPU Powers
            try
            {
                if (!_isCPUPowers)
                    return;

                LoggerHelper.Info($"Sending CPU powers");

                var cpuPowerPackage = CPU.Powers.GetPackage();
                var cpuPowerCores = CPU.Powers.GetCores();
                var cpuPowerGraphics = CPU.Powers.GetGraphics();
                var cpuPowerMemory = CPU.Powers.GetMemory();
                var cpuPowerAll = cpuPowerPackage + cpuPowerCores + cpuPowerGraphics + cpuPowerMemory;

                GetManager().PublishMessage(this, _cpuPowerPackage, cpuPowerPackage.ToString());
                GetManager().PublishMessage(this, _cpuPowerCores, cpuPowerCores.ToString());
                GetManager().PublishMessage(this, _cpuPowerGraphics, cpuPowerGraphics.ToString());
                GetManager().PublishMessage(this, _cpuPowerMemory, cpuPowerMemory.ToString());
                GetManager().PublishMessage(this, _cpuPowerAll, cpuPowerAll.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send powers " + exception);
            }

            // GPU Temperatures
            try
            {
                LoggerHelper.Info($"Sending GPU temperatures");

                var temperatureGPU = GetTemperatureGPU();

                GetManager().PublishMessage(this, _tempGPUTopic, temperatureGPU.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send gpu temperature " + exception);
            }
        }

        public static int GetTemperatureGPU()
        {
            Computer computer = new Computer
            {
                IsGpuEnabled = true
            };

            computer.Open();
            try
            {
                var gpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.GpuNvidia);
                var temperatureSensors = gpu?.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();
                var gpuTempSensor = temperatureSensors?.FirstOrDefault(t => t.Name.ToLower() == "gpu core") ??
                                    temperatureSensors?.First();
                if (gpuTempSensor?.Value != null)
                    return (int)gpuTempSensor.Value;
                return 0;
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Failed to read gpu temperature", e);
                return 0;
            }
        }

        public static void GetSensors()
        {
            Computer computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true
            };

            computer.Open();

            foreach (IHardware hardware in computer.Hardware)
            {
                LoggerHelper.Info("Hardware: {0}", hardware.Name);

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    LoggerHelper.Info("\tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        LoggerHelper.Info("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {
                    LoggerHelper.Info("\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                }
            }
            computer.Close();
        }
    }
}
