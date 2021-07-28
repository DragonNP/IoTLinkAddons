using IOTLinkAPI.Helpers;
using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monitor
{
    public class CPU
    {
        private Computer _computer;

        public HardwareClocks Clocks { get; }
        public HardwareTemperatures Temperatures { get; }
        public HardwareLoad Load { get; }
        public HardwarePowers Powers { get; }

        public CPU()
        {

            _computer = new Computer
            {
                IsCpuEnabled = true
            };

            _computer.Open();

            Clocks = new HardwareClocks(_computer);
            Temperatures = new HardwareTemperatures(_computer);
            Load = new HardwareLoad(_computer);
            Powers = new HardwarePowers(_computer);
        }

        public string GetName()
        {
            try
            {
                var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
                cpu.Update();

                if (cpu.Name != null)
                    return cpu.Name;
                return "";
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Failed to read cpu name", e);
                return "";
            }
        }

        public class HardwareClocks
        {
            private Computer _computer;
            private IHardware _cpu;
            private List<ISensor> _clockSensors;

            public HardwareClocks(Computer computer) {
                _computer = computer;

                _cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
            }

            public int GetCore(int core_num)
            {
                try
                {
                    Update();

                    var sensor = _clockSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu core #" + core_num) ??
                        _clockSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
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
                    Update();

                    var sensor = _clockSensors?.FirstOrDefault(t => t.Name.ToLower() == "bus speed") ??
                        _clockSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu clock bus speed package", e);
                    return 0;
                }
            }

            private void Update()
            {
                _cpu.Update();
                _clockSensors = _cpu?.Sensors.Where(s => s.SensorType == SensorType.Clock).ToList();
            }
        }

        public class HardwareTemperatures
        {
            private Computer _computer;
            private IHardware _cpu;
            private List<ISensor> _tempSensors;

            public HardwareTemperatures(Computer computer)
            {
                _computer = computer;

                _cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
            }

            public int GetCore(int core_num)
            {
                try
                {
                    Update();

                    var sensor = _tempSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu core #" + core_num) ??
                        _tempSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
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
                    Update();

                    var sensor = _tempSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu package") ??
                        _tempSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
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
                    Update();

                    var sensor = _tempSensors?.FirstOrDefault(t => t.Name.ToLower() == "core max") ??
                        _tempSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
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
                    Update();

                    var sensor = _tempSensors?.FirstOrDefault(t => t.Name.ToLower() == "core average") ??
                        _tempSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu temperature average", e);
                    return 0;
                }
            }

            private void Update()
            {
                _cpu.Update();
                _tempSensors = _cpu?.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();
            }
        }

        public class HardwareLoad
        {
            private Computer _computer;
            private IHardware _cpu;
            private List<ISensor> _loadSensors;

            public HardwareLoad(Computer computer)
            {
                _computer = computer;

                _cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
            }

            public int GetCore(int core_num)
            {
                try
                {
                    Update();

                    var sensor = _loadSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu core #" + core_num) ??
                        _loadSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
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
                    Update();

                    var sensor = _loadSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu total") ??
                        _loadSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu load total", e);
                    return 0;
                }
            }

            private void Update()
            {
                _cpu.Update();
                _loadSensors = _cpu?.Sensors.Where(s => s.SensorType == SensorType.Load).ToList();
            }
        }

        public class HardwarePowers
        {
            private Computer _computer;
            private IHardware _cpu;
            private List<ISensor> _powerSensors;

            public HardwarePowers(Computer computer)
            {
                _computer = computer;

                _cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
            }

            public int GetPackage()
            {
                try
                {
                    Update();

                    var sensor = _powerSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu package") ??
                        _powerSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
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
                    Update();

                    var sensor = _powerSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu cores") ??
                        _powerSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
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
                    Update();

                    var sensor = _powerSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu graphics") ??
                        _powerSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
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
                    Update();

                    var sensor = _powerSensors?.FirstOrDefault(t => t.Name.ToLower() == "cpu memory") ??
                        _powerSensors?.First();

                    if (sensor?.Value != null)
                        return (int)sensor.Value;
                    return 0;
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("Failed to read cpu power memory", e);
                    return 0;
                }
            }

            private void Update()
            {
                _cpu.Update();
                _powerSensors = _cpu?.Sensors.Where(s => s.SensorType == SensorType.Power).ToList();
            }
        }
    }
}