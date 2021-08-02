using System;
using System.IO;
using System.Timers;
using IOTLinkAPI.Addons;
using IOTLinkAPI.Configs;
using IOTLinkAPI.Helpers;
using Monitor.Monitors;
using Timer = System.Timers.Timer;

namespace Monitor
{
    public class MonitorService : ServiceAddon
    {
        private Timer _monitorTimer;
        private Configuration _config;

        private Cpu _cpu;
        private Memory _memory;
        private GpuNvidia _gpuNvidia;
        private Storages _storages;

        private bool _isSendedCPUName, _isSendedGPUNvidiaName;
        private string _cpuClocksTopic, _cpuTemperaturesTopic, _cpuPowersTopic;
        private string _memoryDataTopic, _memoryLoadTopic;
        private string _gpuNvidiaClocksTopic, _gpuNvidiaTemperaturesTopic, _gpuNvidiaLoadTopic, _gpuNvidiaControlsTopic, _gpuNvidiaDataTopic, _gpuNvidiaThroughputTopic;
        private string _storagesTopic;

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);

            SetupConfiguration();
            SetupMQTTTopics();

            _cpu = new Cpu();
            _memory = new Memory();
            _gpuNvidia = new GpuNvidia();
            _storages = new Storages();

            _monitorTimer = new Timer
            {
                Interval = 30000
            };
            _monitorTimer.Elapsed += TimerElapsed;
            _monitorTimer.Start();
        }

        void SetupConfiguration()
        {
            var cfgManager = ConfigurationManager.GetInstance();
            var _configPath = Path.Combine(_currentPath, "addon.yaml");
            _config = cfgManager.GetConfiguration(_configPath);
        }

        void SetupMQTTTopics()
        {
            _cpuClocksTopic = "stats/cpu/clocks/";
            _cpuTemperaturesTopic = "stats/cpu/temperatures/";
            _cpuPowersTopic = "stats/cpu/powers/";

            _memoryDataTopic = "stats/memory/data/";
            _memoryLoadTopic = "stats/memory/load/";

            _gpuNvidiaClocksTopic = "stats/gpu_nvidia/clocks/";
            _gpuNvidiaTemperaturesTopic = "stats/gpu_nvidia/temperatures/";
            _gpuNvidiaLoadTopic = "stats/gpu_nvidia/load/";
            _gpuNvidiaControlsTopic = "stats/gpu_nvidia/controls/";
            _gpuNvidiaDataTopic = "stats/gpu_nvidia/data/";
            _gpuNvidiaThroughputTopic = "stats/gpu_nvidia/throughput/";

            _storagesTopic = "stats/storages/{0}/{1}";
        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_config.GetValue("cpu", false) && !_isSendedCPUName)
                PublishCPUName();
            if (_config.GetValue("gpu_nvidia", false) && !_isSendedGPUNvidiaName)
                PublishGPUNvidiaName();

            if (_config.GetValue("cpu", false))
                PublishCPU();
            if (_config.GetValue("memory", false))
                PublishMemory();
            if (_config.GetValue("gpu", false))
                PublishGPU();
            if (_config.GetValue("storages", false))
                PublishStorages();
        }

        public void PublishCPUName()
        {
            var cpuName = _cpu.GetName();
            LoggerHelper.Info($"Sending cpu name: {cpuName}");
            GetManager().PublishMessage(this, "stats/cpu/name", cpuName.ToString());

            _isSendedCPUName = true;
        }

        public void PublishGPUNvidiaName()
        {
            var gpuName = _gpuNvidia.GetName();
            LoggerHelper.Info($"Sending gpu nvidia name: {gpuName}");
            GetManager().PublishMessage(this, "stats/gpu_nvidia/name", gpuName.ToString());

            _isSendedGPUNvidiaName = true;
        }

        void PublishCPU()
        {
            // CPU Clocks
            try
            {
                var sensors = _cpu.GetClocks();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _cpuClocksTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send cpu clocks " + exception);
            }

            // CPU Temperatures
            try
            {
                var sensors = _cpu.GetTemperatures();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _cpuTemperaturesTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send cpu temperatures " + exception);
            }

            // CPU Powers
            try
            {
                var sensors = _cpu.GetPowers();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _cpuPowersTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send powers " + exception);
            }
        }

        void PublishMemory()
        {
            // Memory Data
            try
            {
                var sensors = _memory.GetData();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _memoryDataTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send memory data " + exception);
            }

            // Memory Load
            try
            {
                var sensors = _memory.GetLoad();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _memoryLoadTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send memory load " + exception);
            }
        }

        void PublishGPU()
        {
            // GPU Nvidia Clocks
            try
            {
                var sensors = _gpuNvidia.GetClocks();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _gpuNvidiaClocksTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send gpu nvidia clocks " + exception);
            }

            // GPU Nvidia Temperatures
            try
            {
                var sensors = _gpuNvidia.GetTemperatures();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _gpuNvidiaTemperaturesTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send gpu nvidia temperatures " + exception);
            }

            // GPU Nvidia Load
            try
            {
                var sensors = _gpuNvidia.GetLoad();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _gpuNvidiaLoadTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send gpu nvidia load " + exception);
            }

            // GPU Nvidia Controls
            try
            {
                var sensors = _gpuNvidia.GetControls();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _gpuNvidiaControlsTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send gpu nvidia controls " + exception);
            }

            // GPU Nvidia Data
            try
            {
                var sensors = _gpuNvidia.GetData();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _gpuNvidiaDataTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send gpu nvidia data " + exception);
            }

            // GPU Nvidia Throughput
            try
            {
                var sensors = _gpuNvidia.GetThroughput();
                foreach (var keyvalue in sensors)
                {
                    var name = keyvalue.Key;
                    var value = keyvalue.Value;

                    GetManager().PublishMessage(this, _gpuNvidiaThroughputTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send gpu nvidia throughput " + exception);
            }
        }

        void PublishStorages()
        {
            try
            {
                var all_storages = _storages.GetStorages();
                foreach (var storage in all_storages)
                {
                    string name = "", temp = "", used_space = "";

                    foreach (var sensors in storage)
                    {
                        var sensorType = sensors.Key;
                        var value = sensors.Value;

                        switch (sensorType)
                        {
                            case SensorStorageType.Name:
                                name = value;
                                break;
                            case SensorStorageType.Temperature:
                                temp = value;
                                break;
                            case SensorStorageType.UsedSpace:
                                used_space = value;
                                break;
                        }
                    }

                    GetManager().PublishMessage(this, string.Format(_storagesTopic, name, "temperatue"), temp);
                    GetManager().PublishMessage(this, string.Format(_storagesTopic, name, "used_space"), used_space);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send storages sensors " + exception);
            }
        }
    }
}
