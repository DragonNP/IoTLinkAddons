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

        private bool _isSendedCPUName, _isSendedGPUNvidiaName;
        private string _cpuClocksTopic, _cpuTemperaturesTopic, _cpuPowersTopic;
        private string _memoryDataTopic, _memoryLoadTopic;
        private string _gpuNvidiaClocksTopic, _gpuNvidiaTemperaturesTopic;

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);

            SetupConfiguration();
            SetupMQTTTopics();

            _cpu = new Cpu();
            _memory = new Memory();
            _gpuNvidia = new GpuNvidia();

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
        }

        public void PublishCPUName()
        {
            if (!_config.GetValue("cpu_name", false))
                return;

            var cpuName = _cpu.GetName();
            LoggerHelper.Info($"Sending cpu name: {cpuName}");
            GetManager().PublishMessage(this, "stats/cpu/name", cpuName.ToString());

            _isSendedCPUName = true;
        }

        public void PublishGPUNvidiaName()
        {
            if (!_config.GetValue("gpu_nvidia_name", false))
                return;

            var gpuName = _gpuNvidia.GetName();
            LoggerHelper.Info($"Sending gpu nvidia name: {gpuName}");
            GetManager().PublishMessage(this, "stats/gpu_nvidia/name", gpuName.ToString());

            _isSendedGPUNvidiaName = true;
        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isSendedCPUName)
                PublishCPUName();
            if (!_isSendedGPUNvidiaName)
                PublishGPUNvidiaName();

            // CPU Clocks
            try
            {
                if (!_config.GetValue("cpu_clocks", false))
                    return;

                LoggerHelper.Info($"Sending CPU clocks");

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
                if (!_config.GetValue("cpu_temps", false))
                    return;

                LoggerHelper.Info($"Sending CPU temperatures");

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
                if (!_config.GetValue("cpu_powers", false))
                    return;

                LoggerHelper.Info($"Sending CPU powers");

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

            // Memory Data
            try
            {
                if (!_config.GetValue("memory_data", false))
                    return;

                LoggerHelper.Info($"Sending Memory data");

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
                if (!_config.GetValue("memory_load", false))
                    return;

                LoggerHelper.Info($"Sending Memory load");

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

            // GPU Nvidia Clocks
            try
            {
                if (!_config.GetValue("gpu_nvidia_clocks", false))
                    return;

                LoggerHelper.Info($"Sending GPU Nvidia clocks");

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
                if (!_config.GetValue("gpu_nvidia_temperatures", false))
                    return;

                LoggerHelper.Info($"Sending GPU Nvidia temperatures");

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
        }
    }
}
