using System;
using System.IO;
using System.Timers;
using IOTLinkAPI.Addons;
using IOTLinkAPI.Configs;
using IOTLinkAPI.Helpers;
using Timer = System.Timers.Timer;

namespace Monitor
{
    public class MonitorService : ServiceAddon
    {
        private Timer _monitorTimer;

        private Configuration _config;
        private MQTTConfiguration _mqttConfig;

        private CPU _cpu;

        private bool _isSendedCPUName;

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);

            var cfgManager = ConfigurationManager.GetInstance();
            var _configPath = Path.Combine(_currentPath, "addon.yaml");
            _config = cfgManager.GetConfiguration(_configPath);

            _mqttConfig = new MQTTConfiguration();

            _cpu = new CPU();

            PublishCPUName();

            _monitorTimer = new Timer
            {
                Interval = 10000
            };
            _monitorTimer.Elapsed += TimerElapsed;
            _monitorTimer.Start();
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

        private void TimerElapsed(object sender, ElapsedEventArgs e)
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

                    GetManager().PublishMessage(this, _mqttConfig.CpuClocksTopic+name, value);
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

                    GetManager().PublishMessage(this, _mqttConfig.CpuTemperaturesTopic + name, value);
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

                    GetManager().PublishMessage(this, _mqttConfig.CpuPowersTopic + name, value);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send powers " + exception);
            }
        }
    }
}
