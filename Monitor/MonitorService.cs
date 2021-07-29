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

        private CPU _cpu;
        private Memory _memory;

        private bool _isSendedCPUName;
        private string _cpuClocksTopic, _cpuTemperaturesTopic, _cpuPowersTopic;
        private string _memoryDataTopic, _memoryLoadTopic;

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);

            SetupConfiguration();
            SetupMQTTTopics();

            _cpu = new CPU();
            _memory = new Memory();

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

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isSendedCPUName)
                PublishCPUName();

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
        }
    }
}
