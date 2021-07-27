using IOTLinkAPI.Helpers;
using LibreHardwareMonitor.Hardware;
using System;
using System.Linq;

namespace Monitor
{
    public static class CPU
    {
        public static string Name()
        {
            Computer computer = new Computer
            {
                IsCpuEnabled = true
            };

            computer.Open();
            try
            {
                var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                var cpuName = cpu.Name;
                if (cpuName != null)
                    return cpuName;
                return "";
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Failed to read cpu name", e);
                return "";
            }
        }

        public static class Clocks
        {
            public static int GetCore(int core_num)
            {
                Computer computer = new Computer
                {
                    IsCpuEnabled = true
                };

                computer.Open();
                try
                {
                    var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                    var clockSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Clock).ToList();
                    var cpuClockCore = clockSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu core #" + core_num) ??
                                        clockSensors?.First();
                    if (cpuClockCore?.Value != null)
                        return (int)cpuClockCore.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu clock core " + core_num, e);
                    return 0;
                }
            }

            public static int GetBusSpeed()
            {
                Computer computer = new Computer
                {
                    IsCpuEnabled = true
                };

                computer.Open();
                try
                {
                    var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                    var clockSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Clock).ToList();
                    var cpuBusSpeed = clockSensors?.FirstOrDefault(t => t.Name.ToLower() == "bus speed") ??
                                        clockSensors?.First();
                    if (cpuBusSpeed?.Value != null)
                        return (int)cpuBusSpeed.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu clock bus speed package", e);
                    return 0;
                }
            }
        }
        
        public static class Temperatures
        {
            public static int GetCore(int core_num)
            {
                Computer computer = new Computer
                {
                    IsCpuEnabled = true
                };

                computer.Open();
                try
                {
                    var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                    var tempSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();
                    var cpuTempCore = tempSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu core #" + core_num) ??
                                        tempSensors?.First();
                    if (cpuTempCore?.Value != null)
                        return (int)cpuTempCore.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu temperature core " + core_num, e);
                    return 0;
                }
            }

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
                    var tempSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();
                    var cpuTempPackage = tempSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu package") ??
                                        tempSensors?.First();
                    if (cpuTempPackage?.Value != null)
                        return (int)cpuTempPackage.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu temperature package", e);
                    return 0;
                }
            }

            public static int GetMax()
            {
                Computer computer = new Computer
                {
                    IsCpuEnabled = true
                };

                computer.Open();
                try
                {
                    var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                    var tempSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();
                    var cpuTempMax = tempSensors?.FirstOrDefault(t => t.Name.ToLower() == "core max") ??
                                        tempSensors?.First();
                    if (cpuTempMax?.Value != null)
                        return (int)cpuTempMax.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu temperature max", e);
                    return 0;
                }
            }

            public static int GetAverage()
            {
                Computer computer = new Computer
                {
                    IsCpuEnabled = true
                };

                computer.Open();
                try
                {
                    var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                    var tempSensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();
                    var cpuTempAverage = tempSensors?.FirstOrDefault(t => t.Name.ToLower() == "core average") ??
                                        tempSensors?.First();
                    if (cpuTempAverage?.Value != null)
                        return (int)cpuTempAverage.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu temperature average", e);
                    return 0;
                }
            }
        }

        public static class Load {
            public static int GetCore(int core_num)
            {
                Computer computer = new Computer
                {
                    IsCpuEnabled = true
                };

                computer.Open();
                try
                {
                    var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                    var sensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Load).ToList();
                    var cpuLoadCore = sensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu core #" + core_num) ??
                                        sensors?.First();

                    if (cpuLoadCore?.Value != null)
                        return (int)cpuLoadCore.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu load core " + core_num, e);
                    return 0;
                }
            }
            public static int GetTotal()
            {
                Computer computer = new Computer
                {
                    IsCpuEnabled = true
                };

                computer.Open();
                try
                {
                    var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                    var sensors = cpu?.Sensors.Where(s => s.SensorType == SensorType.Load).ToList();
                    var cpuLoadTotal = sensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu total") ??
                                        sensors?.First();
                    if (cpuLoadTotal?.Value != null)
                        return (int)cpuLoadTotal.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu load total", e);
                    return 0;
                }
            }
        }

        public static class Powers
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
}
