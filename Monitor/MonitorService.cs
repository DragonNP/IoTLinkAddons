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
        private string _tempGPUTopic, _tempDriveTopic;
        private string _cpuPowerPackage, _cpuPowerCores, _cpuPowerGraphics, _cpuPowerMemory, _cpuPowerAll;
        private string _cpuTemperatureCore, _cpuTemperatureMax, _cpuTemperatureAverage, _cpuTemperaturePackage;
        private string _cpuClockCore, _cpuClockBusSpeed;

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);
            GetSensors();

            _tempGPUTopic = "stats/gpu/temperature";
            _tempDriveTopic = "stats/drive/temperature";

            GetManager().PublishDiscoveryMessage(this, _tempGPUTopic, "GPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, _tempDriveTopic, "Drive", DiscoveryOptionsTemperature());

            _cpuPowerPackage = "stats/cpu/powers/package";
            _cpuPowerCores = "stats/cpu/powers/cores";
            _cpuPowerGraphics = "stats/cpu/powers/graphics";
            _cpuPowerMemory = "stats/cpu/powers/memory";
            _cpuPowerAll = "stats/cpu/powers/all";

            GetManager().PublishDiscoveryMessage(this, _cpuPowerPackage, "CPU", DiscoveryOptionsPower());
            GetManager().PublishDiscoveryMessage(this, _cpuPowerCores, "CPU", DiscoveryOptionsPower());
            GetManager().PublishDiscoveryMessage(this, _cpuPowerGraphics, "CPU", DiscoveryOptionsPower());
            GetManager().PublishDiscoveryMessage(this, _cpuPowerMemory, "CPU", DiscoveryOptionsPower());
            GetManager().PublishDiscoveryMessage(this, _cpuPowerAll, "CPU", DiscoveryOptionsPower());

            _cpuTemperatureCore = "stats/cpu/temperatures/core{0}";
            _cpuTemperatureMax = "stats/cpu/temperatures/max";
            _cpuTemperatureAverage = "stats/cpu/temperatures/average";
            _cpuTemperaturePackage = "stats/cpu/temperatures/package";

            GetManager().PublishDiscoveryMessage(this, string.Format(_cpuTemperatureCore, 1), "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, string.Format(_cpuTemperatureCore, 2), "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, string.Format(_cpuTemperatureCore, 3), "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, string.Format(_cpuTemperatureCore, 4), "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, _cpuTemperatureMax, "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, _cpuTemperatureAverage, "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, _cpuTemperaturePackage, "CPU", DiscoveryOptionsTemperature());

            _cpuClockCore = "stats/cpu/clocks/core{0}";
            _cpuClockBusSpeed = "stats/cpu/clocks/bus-speed";

            GetManager().PublishDiscoveryMessage(this, string.Format(_cpuClockCore, 1), "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, string.Format(_cpuClockCore, 2), "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, string.Format(_cpuClockCore, 3), "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, string.Format(_cpuClockCore, 4), "CPU", DiscoveryOptionsTemperature());
            GetManager().PublishDiscoveryMessage(this, _cpuClockBusSpeed, "CPU", DiscoveryOptionsTemperature());

            _monitorTimer = new Timer();
            _monitorTimer.Interval = 10000;
            _monitorTimer.Elapsed += TimerElapsed;
            _monitorTimer.Start();
        }

        public static HassDiscoveryOptions DiscoveryOptionsTemperature()
        {
            return new HassDiscoveryOptions
            {
                Id = "Temperature",
                Unit = "°C",
                Name = "Temperature",
                Component = HomeAssistantComponent.Sensor,
                Icon = "mdi:thermometer"
            };
        }

        public static HassDiscoveryOptions DiscoveryOptionsPower()
        {
            return new HassDiscoveryOptions
            {
                Id = "Power",
                Unit = "W",
                Name = "Power",
                Component = HomeAssistantComponent.Sensor,
                Icon = "mdi:power-plug"
            };
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Clocks
            try
            {
                for (int core_num = 1; core_num < 5; core_num++)
                {
                    var cpuClockCore = CPU.Clocks.GetCore(core_num);
                    LoggerHelper.Info($"Sending {cpuClockCore} MHz");
                    GetManager().PublishMessage(this, string.Format(_cpuClockCore, core_num), cpuClockCore.ToString());
                }

                var cpuBusSpeed = CPU.Clocks.GetBusSpeed();
                LoggerHelper.Info($"Sending {_cpuClockBusSpeed} MHz");
                GetManager().PublishMessage(this, _cpuClockBusSpeed, cpuBusSpeed.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send cpu clocks " + exception);
            }

            // Temperatures
            try
            {
                for (int core_num = 1; core_num < 5; core_num++)
                {
                    var cpuTempCore = CPU.Temperatures.GetCore(core_num);
                    LoggerHelper.Info($"Sending {cpuTempCore} celsius");
                    GetManager().PublishMessage(this, string.Format(_cpuTemperatureCore, core_num), cpuTempCore.ToString());
                }

                var cpuTempPackage = CPU.Temperatures.GetPackage();
                LoggerHelper.Info($"Sending {cpuTempPackage} celsius");
                GetManager().PublishMessage(this, _cpuTemperaturePackage, cpuTempPackage.ToString());

                var cpuTempMax = CPU.Temperatures.GetMax();
                LoggerHelper.Info($"Sending {cpuTempMax} celsius");
                GetManager().PublishMessage(this, _cpuTemperatureMax, cpuTempMax.ToString());

                var cpuTempAverage = CPU.Temperatures.GetAverage();
                LoggerHelper.Info($"Sending {cpuTempAverage} celsius");
                GetManager().PublishMessage(this, _cpuTemperatureAverage, cpuTempAverage.ToString());

                var temperatureGPU = GetTemperatureGPU();
                LoggerHelper.Info($"Sending {temperatureGPU} celsius");
                GetManager().PublishMessage(this, _tempGPUTopic, temperatureGPU.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send temperature " + exception);
            }

            // Powers
            try
            {
                // CPU
                var cpuPowerPackage = CPU.Powers.GetPackage();
                LoggerHelper.Info($"Sending {cpuPowerPackage} watt");
                GetManager().PublishMessage(this, _cpuPowerPackage, cpuPowerPackage.ToString());

                var cpuPowerCores = CPU.Powers.GetCores();
                LoggerHelper.Info($"Sending {cpuPowerCores} watt");
                GetManager().PublishMessage(this, _cpuPowerCores, cpuPowerCores.ToString());

                var cpuPowerGraphics = CPU.Powers.GetGraphics();
                LoggerHelper.Info($"Sending {cpuPowerGraphics} watt");
                GetManager().PublishMessage(this, _cpuPowerGraphics, cpuPowerGraphics.ToString());

                var cpuPowerMemory = CPU.Powers.GetMemory();
                LoggerHelper.Info($"Sending {cpuPowerMemory} watt");
                GetManager().PublishMessage(this, _cpuPowerMemory, cpuPowerMemory.ToString());

                var cpuPowerAll = cpuPowerPackage + cpuPowerCores + cpuPowerGraphics + cpuPowerMemory;
                LoggerHelper.Info($"Sending {cpuPowerAll} watt");
                GetManager().PublishMessage(this, _cpuPowerAll, cpuPowerAll.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send powers " + exception);
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
