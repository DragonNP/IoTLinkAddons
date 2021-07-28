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
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // CPU Clocks
            try
            {
                if (!_config.GetValue("cpu_clocks", false))
                    return;

                LoggerHelper.Info($"Sending CPU clocks");

                for (int core_num = 1; core_num < 5; core_num++)
                {
                    var cpuClockCore = _cpu.Clocks.GetCore(core_num);
                    GetManager().PublishMessage(this, string.Format(_mqttConfig.GetClockTopic("core"), core_num), cpuClockCore.ToString());
                }

                var cpuBusSpeed = _cpu.Clocks.GetBusSpeed();
                GetManager().PublishMessage(this, _mqttConfig.GetClockTopic("bus-speed"), cpuBusSpeed.ToString());
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

                LoggerHelper.Info($"Sending CPU temperature");

                for (int core_num = 1; core_num < 5; core_num++)
                {
                    var cpuTempCore = _cpu.Temperatures.GetCore(core_num);
                    GetManager().PublishMessage(this, string.Format(_mqttConfig.GetTempTopic("core"), core_num), cpuTempCore.ToString());
                }

                var cpuTempPackage = _cpu.Temperatures.GetPackage();
                var cpuTempMax = _cpu.Temperatures.GetMax();
                var cpuTempAverage = _cpu.Temperatures.GetAverage();

                GetManager().PublishMessage(this, _mqttConfig.GetTempTopic("package"), cpuTempPackage.ToString());
                GetManager().PublishMessage(this, _mqttConfig.GetTempTopic("max"), cpuTempMax.ToString());
                GetManager().PublishMessage(this, _mqttConfig.GetTempTopic("average"), cpuTempAverage.ToString());


            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send cpu temperatures " + exception);
            }

            // CPU Load
            try
            {
                if (!_config.GetValue("cpu_load", false))
                    return;

                LoggerHelper.Info($"Sending CPU load");

                for (int core_num = 1; core_num < 5; core_num++)
                {
                    var cpuLoadCore = _cpu.Load.GetCore(core_num);
                    GetManager().PublishMessage(this, string.Format(_mqttConfig.GetLoadTopic("core"), core_num), cpuLoadCore.ToString());
                }

                var cpuLoadTotal = _cpu.Load.GetTotal();
                GetManager().PublishMessage(this, _mqttConfig.GetLoadTopic("total"), cpuLoadTotal.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send cpu load " + exception);
            }

            // CPU Powers
            try
            {
                if (!_config.GetValue("cpu_powers", false))
                    return;

                LoggerHelper.Info($"Sending CPU powers");

                var cpuPowerPackage = _cpu.Powers.GetPackage();
                var cpuPowerCores = _cpu.Powers.GetCores();
                var cpuPowerGraphics = _cpu.Powers.GetGraphics();
                var cpuPowerMemory = _cpu.Powers.GetMemory();
                var cpuPowerAll = cpuPowerPackage + cpuPowerCores + cpuPowerGraphics + cpuPowerMemory;

                GetManager().PublishMessage(this, _mqttConfig.GetPowerTopic("package"), cpuPowerPackage.ToString());
                GetManager().PublishMessage(this, _mqttConfig.GetPowerTopic("cores"), cpuPowerCores.ToString());
                GetManager().PublishMessage(this, _mqttConfig.GetPowerTopic("graphics"), cpuPowerGraphics.ToString());
                GetManager().PublishMessage(this, _mqttConfig.GetPowerTopic("memory"), cpuPowerMemory.ToString());
                GetManager().PublishMessage(this, _mqttConfig.GetPowerTopic("all"), cpuPowerAll.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send powers " + exception);
            }
        }
    }
}
