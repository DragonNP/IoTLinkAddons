using IOTLinkAPI.Helpers;
using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor
{
    class CPUPowers
    {
        public static int GetPackage()
        {
            Computer computer = new Computer
            {
                IsCpuEnabled = true
            };

            computer.Open();
            try
            {
                var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                var powerSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Power).ToList();
                var cpuPowerPackage = powerSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu package") ??
                                    powerSensors?.First();
                if (cpuPowerPackage?.Value != null)
                    return (int)cpuPowerPackage.Value;
                return 0;
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Failed to read cpu power package", e);
                return 0;
            }
        }
        public static int GetCores()
        {
            Computer computer = new Computer
            {
                IsCpuEnabled = true
            };

            computer.Open();
            try
            {
                var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                var powerSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Power).ToList();
                var cpuPowerCores = powerSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu cores") ??
                                    powerSensors?.First();
                if (cpuPowerCores?.Value != null)
                    return (int)cpuPowerCores.Value;
                return 0;
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Failed to read cpu power cores", e);
                return 0;
            }
        }

        public static int GetGraphics()
        {
            Computer computer = new Computer
            {
                IsCpuEnabled = true
            };

            computer.Open();
            try
            {
                var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                var powerSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Power).ToList();
                var cpuPowerGraphics = powerSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu graphics") ??
                                    powerSensors?.First();
                if (cpuPowerGraphics?.Value != null)
                    return (int)cpuPowerGraphics.Value;
                return 0;
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Failed to read cpu power graphics", e);
                return 0;
            }
        }

        public static int GetMemory()
        {
            Computer computer = new Computer
            {
                IsCpuEnabled = true
            };

            computer.Open();
            try
            {
                var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                var powerSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Power).ToList();
                var cpuMemoryGraphics = powerSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu memory") ??
                                    powerSensors?.First();
                if (cpuMemoryGraphics?.Value != null)
                    return (int)cpuMemoryGraphics.Value;
                return 0;
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Failed to read cpu power memory", e);
                return 0;
            }
        }
    }
}
