using IOTLinkAPI.Helpers;
using LibreHardwareMonitor.Hardware;
using System;
using System.Linq;

namespace Monitor
{
    public class CPU
    {
        public Computer _computer;

        public Clocks _clocks;
        public Temperatures _temperatures;
        public Load _load;
        public Powers _powers;

        public CPU()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true
            };

            _computer.Accept(new UpdateVisitor());
            _computer.Open();

            _clocks = new Clocks(_computer);
            _temperatures = new Temperatures(_computer);
            _load = new Load(_computer);
            _powers = new Powers(_computer);
        }

        public string GetName()
        {
            try
            {
                var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

        public class Clocks
        {
            private Computer _computer;

            public Clocks(Computer computer) => _computer = computer;

            public int GetCore(int core_num)
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

            public int GetBusSpeed()
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

        public class Temperatures
        {
            private Computer _computer;

            public Temperatures(Computer computer) => _computer = computer;

            public int GetCore(int core_num)
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

            public int GetPackage()
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

            public int GetMax()
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

            public int GetAverage()
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

        public class Load {
            private Computer _computer;

            public Load(Computer computer) => _computer = computer;

            public int GetCore(int core_num)
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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
            
            public int GetTotal()
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

        public class Powers
        {
            private Computer _computer;

            public Powers(Computer computer) => _computer = computer;

            public  int GetPackage()
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

            public int GetCores()
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

            public int GetGraphics()
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

            public int GetMemory()
            {
                try
                {
                    var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subhardware in hardware.SubHardware) subhardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}
