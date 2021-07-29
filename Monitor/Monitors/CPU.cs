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
        private IHardware _cpu;

        public CPU()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true
            };
            _computer.Open();
            _cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
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

        public IDictionary<string, string> GetClocks()
        {
            _cpu.Update();

            var readed_sensors = new Dictionary<string, string> { };
            var clock_sensors = _cpu?.Sensors.Where(s => s.SensorType == SensorType.Clock).ToList();

            foreach (var sensor in clock_sensors)
            {
                var name = sensor.Name;
                var value = sensor.Value?.ToString("0.#");

                name = name.ToLower().Replace("#", "");

                readed_sensors.Add(name, value);
            }

            return readed_sensors;
        }

        public IDictionary<string, string> GetTemperatures()
        {
            _cpu.Update();

            var readed_sensors = new Dictionary<string, string> { };
            var temp_sensors = _cpu?.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();

            foreach (var sensor in temp_sensors)
            {
                var name = sensor.Name;
                var value = sensor.Value?.ToString("0.#");

                name = name.ToLower().Replace("#", "");

                readed_sensors.Add(name, value);
            }

            return readed_sensors;
        }

        public IDictionary<string, string> GetPowers()
        {
            _cpu.Update();

            var readed_sensors = new Dictionary<string, string> { };
            var power_sensors = _cpu?.Sensors.Where(s => s.SensorType == SensorType.Power).ToList();

            foreach (var sensor in power_sensors)
            {
                var name = sensor.Name;
                var value = sensor.Value?.ToString("0.#");

                name = name.ToLower().Replace("#", "");

                readed_sensors.Add(name, value);
            }

            return readed_sensors;
        }
    }
}