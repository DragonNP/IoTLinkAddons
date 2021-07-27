using System;
using System.Linq;
using System.Timers;
using IOTLinkAPI.Addons;
using IOTLinkAPI.Helpers;
using IOTLinkAPI.Platform.HomeAssistant;
using LibreHardwareMonitor.Hardware;
using Timer = System.Timers.Timer;

namespace TemperatureMonitor
{
    public class TemperatureService : ServiceAddon
    {
        private Timer _monitorTimer;
        private string _tempCPUTopic, _tempGPUTopic, _tempDriveTopic;
        private string _cpuPowerPackage, _cpuPowerCores, _cpuPowerGraphics, _cpuPowerMemory, _cpuPowerAll;

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);
            GetSensors();

            _tempCPUTopic = "stats/cpu/temperature";
            _tempGPUTopic = "stats/gpu/temperature";
            _tempDriveTopic = "stats/drive/temperature";

            GetManager().PublishDiscoveryMessage(this, _tempCPUTopic, "CPU", DiscoveryOptionsTemperature());
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
            try
            {
                var temperatureCPU = GetTemperatureCPU();
                LoggerHelper.Info($"Sending {temperatureCPU} celsius");
                GetManager().PublishMessage(this, _tempCPUTopic, temperatureCPU.ToString());

                var temperatureGPU = GetTemperatureGPU();
                LoggerHelper.Info($"Sending {temperatureGPU} celsius");
                GetManager().PublishMessage(this, _tempGPUTopic, temperatureGPU.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send temperature " + exception);
            }

            try
            {
                // CPU
                var cpuPowerPackage = CPUPowers.GetPackage();
                LoggerHelper.Info($"Sending {cpuPowerPackage} watt");
                GetManager().PublishMessage(this, _cpuPowerPackage, cpuPowerPackage.ToString());

                var cpuPowerCores = CPUPowers.GetCores();
                LoggerHelper.Info($"Sending {cpuPowerCores} watt");
                GetManager().PublishMessage(this, _cpuPowerCores, cpuPowerCores.ToString());

                var cpuPowerGraphics = CPUPowers.GetGraphics();
                LoggerHelper.Info($"Sending {cpuPowerGraphics} watt");
                GetManager().PublishMessage(this, _cpuPowerGraphics, cpuPowerGraphics.ToString());

                var cpuPowerMemory = CPUPowers.GetMemory();
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


        public static int GetTemperatureCPU()
        {
            Computer computer = new Computer
            {
                IsCpuEnabled = true
            };

            computer.Open();
            try
            {
                var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                var temperatureSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();
                var cpuTempSensor = temperatureSensors?.FirstOrDefault(t => t.Name.ToLower() == "core average") ??
                                    temperatureSensors?.First();
                if (cpuTempSensor?.Value != null)
                    return (int)cpuTempSensor.Value;
                return 0;
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Failed to read cpu temperature", e);
                return 0;
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
