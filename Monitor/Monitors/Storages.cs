using LibreHardwareMonitor.Hardware;
using System.Collections.Generic;
using System.Linq;


namespace Monitor.Monitors
{
    class Storages
    {
        private Computer _computer;

        public Storages()
        {
            _computer = new Computer
            {
                IsStorageEnabled = true
            };
            _computer.Open();
        }

        public List<Dictionary<SensorStorageType, string>> GetStorages()
        {
            var all_storages = _computer.Hardware.Where(h => h.HardwareType == HardwareType.Storage);
            var list_sorted_storages = new List<Dictionary<SensorStorageType, string>> { };

            foreach (var storage in all_storages)
            {
                storage.Update();

                var load_sensors = storage.Sensors.Where(h => h.SensorType == SensorType.Load);
                var list_storage = new Dictionary<SensorStorageType, string> { };

                var name = storage.Name;
                var temperature = storage.Sensors.FirstOrDefault(h => h.SensorType == SensorType.Temperature).Value?.ToString("0.#");
                var used_space = load_sensors.FirstOrDefault(h => h.Name.ToLower() == "used space").Value?.ToString("0.#");

                list_storage.Add(SensorStorageType.Name, name);
                list_storage.Add(SensorStorageType.Temperature, temperature);
                list_storage.Add(SensorStorageType.UsedSpace, used_space);

                list_sorted_storages.Add(list_storage);
            }

            return list_sorted_storages;
        }
    }

    enum SensorStorageType
    {
        Name,
        Temperature,
        UsedSpace
    }
}
