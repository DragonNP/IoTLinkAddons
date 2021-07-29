using IOTLinkAPI.Helpers;
using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monitor.Monitors
{
    class GpuNvidia
    {
        private Computer _computer;
        private IHardware _gpu;

        public GpuNvidia()
        {
            _computer = new Computer
            {
                IsGpuEnabled = true
            };
            _computer.Open();
            _gpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.GpuNvidia);
        }

        public string GetName()
        {
            try
            {
                _gpu.Update();

                if (_gpu.Name != null)
                    return _gpu.Name;
                return "";
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Failed to read gpu nvidia name", e);
                return "";
            }
        }

        public IDictionary<string, string> GetClocks()
        {
            _gpu.Update();

            var readed_sensors = new Dictionary<string, string> { };
            var clock_sensors = _gpu?.Sensors.Where(s => s.SensorType == SensorType.Clock).ToList();

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
            _gpu.Update();

            var readed_sensors = new Dictionary<string, string> { };
            var temp_sensors = _gpu?.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();

            foreach (var sensor in temp_sensors)
            {
                var name = sensor.Name;
                var value = sensor.Value?.ToString("0.#");

                name = name.ToLower().Replace("#", "");

                readed_sensors.Add(name, value);
            }

            return readed_sensors;
        }

        public IDictionary<string, string> GetLoad()
        {
            _gpu.Update();

            var readed_sensors = new Dictionary<string, string> { };
            var load_sensors = _gpu?.Sensors.Where(s => s.SensorType == SensorType.Load).ToList();

            foreach (var sensor in load_sensors)
            {
                var name = sensor.Name;
                var value = sensor.Value?.ToString("0.#");

                name = name.ToLower().Replace("#", "");

                if (name.Contains("d3d"))
                    continue;

                readed_sensors.Add(name, value);
            }

            return readed_sensors;
        }
    }
}
