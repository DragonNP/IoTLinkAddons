﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private string _cpuClockCore, _cpuClockBusSpeed;
        private string _cpuTemperatureCore, _cpuTemperatureMax, _cpuTemperatureAverage, _cpuTemperaturePackage;
        private string _cpuLoadCore, _cpuLoadTotal;
        private string _cpuPowerPackage, _cpuPowerCores, _cpuPowerGraphics, _cpuPowerMemory, _cpuPowerAll;

        private Configuration _config;

        private CPU _cpu;

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);

            var cfgManager = ConfigurationManager.GetInstance();
            var _configPath = Path.Combine(_currentPath, "addon.yaml");
            _config = cfgManager.GetConfiguration(_configPath);

            _cpu = new CPU();

            SetupCPUName();
            SetupCPUClocks();
            SetupCPUTemperatures();
            SetupCPULoad();
            SetupCPUPowers();

            _monitorTimer = new Timer();
            _monitorTimer.Interval = 10000;
            _monitorTimer.Elapsed += TimerElapsed;
            _monitorTimer.Start();
        }

        public void SetupCPUName()
        {
            if (!_config.GetValue("cpu_name", false))
                return;

            var cpuName = _cpu.GetName();
            LoggerHelper.Info($"Sending cpu name: {cpuName}");
            GetManager().PublishMessage(this, "stats/cpu/name", cpuName.ToString());
        }

        public void SetupCPUClocks()
        {
            if (!_config.GetValue("cpu_clocks", false))
                return;

            _cpuClockCore = "stats/cpu/clocks/core{0}";
            _cpuClockBusSpeed = "stats/cpu/clocks/bus-speed";
        }

        public void SetupCPUTemperatures()
        {
            if (!_config.GetValue("cpu_temps", false))
                return;

            _cpuTemperatureCore = "stats/cpu/temperatures/core{0}";
            _cpuTemperatureMax = "stats/cpu/temperatures/max";
            _cpuTemperatureAverage = "stats/cpu/temperatures/average";
            _cpuTemperaturePackage = "stats/cpu/temperatures/package";
        }

        public void SetupCPULoad()
        {
            if (!_config.GetValue("cpu_load", false))
                return;

            _cpuLoadCore = "stats/cpu/load/core{0}";
            _cpuLoadTotal = "stats/cpu/load/total";

        }

        public void SetupCPUPowers()
        {
            if (!_config.GetValue("cpu_powers", false))
                return;

            _cpuPowerPackage = "stats/cpu/powers/package";
            _cpuPowerCores = "stats/cpu/powers/cores";
            _cpuPowerGraphics = "stats/cpu/powers/graphics";
            _cpuPowerMemory = "stats/cpu/powers/memory";
            _cpuPowerAll = "stats/cpu/powers/all";
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
                    var cpuClockCore = _cpu._clocks.GetCore(core_num);
                    GetManager().PublishMessage(this, string.Format(_cpuClockCore, core_num), cpuClockCore.ToString());
                }

                var cpuBusSpeed = _cpu._clocks.GetBusSpeed();
                GetManager().PublishMessage(this, _cpuClockBusSpeed, cpuBusSpeed.ToString());
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
                    var cpuTempCore = _cpu._temperatures.GetCore(core_num);
                    GetManager().PublishMessage(this, string.Format(_cpuTemperatureCore, core_num), cpuTempCore.ToString());
                }

                var cpuTempPackage = _cpu._temperatures.GetPackage();
                var cpuTempMax = _cpu._temperatures.GetMax();
                var cpuTempAverage = _cpu._temperatures.GetAverage();

                GetManager().PublishMessage(this, _cpuTemperaturePackage, cpuTempPackage.ToString());
                GetManager().PublishMessage(this, _cpuTemperatureMax, cpuTempMax.ToString());
                GetManager().PublishMessage(this, _cpuTemperatureAverage, cpuTempAverage.ToString());


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
                    var cpuLoadCore = _cpu._load.GetCore(core_num);
                    GetManager().PublishMessage(this, string.Format(_cpuLoadCore, core_num), cpuLoadCore.ToString());
                }

                var cpuLoadTotal = _cpu._load.GetTotal();
                GetManager().PublishMessage(this, _cpuLoadTotal, cpuLoadTotal.ToString());
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

                var cpuPowerPackage = _cpu._powers.GetPackage();
                var cpuPowerCores = _cpu._powers.GetCores();
                var cpuPowerGraphics = _cpu._powers.GetGraphics();
                var cpuPowerMemory = _cpu._powers.GetMemory();
                var cpuPowerAll = cpuPowerPackage + cpuPowerCores + cpuPowerGraphics + cpuPowerMemory;

                GetManager().PublishMessage(this, _cpuPowerPackage, cpuPowerPackage.ToString());
                GetManager().PublishMessage(this, _cpuPowerCores, cpuPowerCores.ToString());
                GetManager().PublishMessage(this, _cpuPowerGraphics, cpuPowerGraphics.ToString());
                GetManager().PublishMessage(this, _cpuPowerMemory, cpuPowerMemory.ToString());
                GetManager().PublishMessage(this, _cpuPowerAll, cpuPowerAll.ToString());
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Failed to send powers " + exception);
            }
        }
    }
}
