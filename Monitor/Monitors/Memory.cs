using LibreHardwareMonitor.Hardware;
using System.Collections.Generic;
using System.Linq;

namespace Monitor.Monitors
{
    class Memory
    {
        private Computer _computer;
        private IHardware _memory;

        public Memory()
        {
            _computer = new Computer
            {
                IsMemoryEnabled = true
            };
            _computer.Open();
            _memory = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Memory);
        }

        public IDictionary<string, string> GetData()
        {
            _memory.Update();

            var readed_sensors = new Dictionary<string, string> { };
            var data_sensors = _memory?.Sensors.Where(s => s.SensorType == SensorType.Data).ToList();

            foreach (var sensor in data_sensors)
            {
                var name = sensor.Name;
                var value = sensor.Value?.ToString("0.#");

                name = name.ToLower();

                readed_sensors.Add(name, value);
            }

            return readed_sensors;
        }

        public IDictionary<string, string> GetLoad()
        {
            _memory.Update();

            var readed_sensors = new Dictionary<string, string> { };
            var load_sensors = _memory?.Sensors.Where(s => s.SensorType == SensorType.Load).ToList();

            foreach (var sensor in load_sensors)
            {
                var name = sensor.Name;
                var value = sensor.Value?.ToString("0.#");

                name = name.ToLower();

                readed_sensors.Add(name, value);
            }

            return readed_sensors;
        }
    }
}
