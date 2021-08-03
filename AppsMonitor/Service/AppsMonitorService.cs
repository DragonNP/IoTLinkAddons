using AppsMonitor.Common.Configs;
using AppsMonitor.Common.Helpers;
using AppsMonitor.Common.Processes;
using IOTLinkAPI.Addons;
using IOTLinkAPI.Configs;
using IOTLinkAPI.Helpers;
using IOTLinkAPI.Platform.Events.MQTT;
using IOTLinkAPI.Platform.HomeAssistant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace AppsMonitor
{
    public class AppsMonitorService : ServiceAddon
    {
        private Configuration _config;
        private Timer _monitorTimer;
        private List<ProcessMonitor> _monitors = new List<ProcessMonitor>();

        private readonly double _timerInterval = 3000;
        private readonly string PAYLOAD_ON = "ON";
        private readonly string PAYLOAD_OFF = "OFF";

        public override void Init(IAddonManager addonManager)
        {
            base.Init(addonManager);

            var cfgManager = ConfigurationManager.GetInstance();
            var _configPath = Path.Combine(_currentPath, "addon.yaml");
            _config = cfgManager.GetConfiguration(_configPath);

            Restart();
        }

        private bool IsAddonEnabled()
        {
            if (_config == null || !_config.GetValue("enabled", false))
            {
                CleanTimers();
                return false;
            }

            return true;
        }

        private void Restart()
        {
            if (!IsAddonEnabled())
                return;

            SetupDiscovery();
            SetupRequestEvents();
            CheckAllMonitors(true);
            StartTimers();
        }

        private void SetupDiscovery()
        {
            List<Configuration> monitorConfigurations = _config.GetConfigurationList("monitors");

            if (monitorConfigurations == null || monitorConfigurations.Count == 0)
            {
                LoggerHelper.Info("AppsMonitorService::SetupMonitors() - Monitoring list is empty.");
                return;
            }

            foreach (Configuration monitorConfiguration in monitorConfigurations)
            {
                try
                {
                    ProcessMonitor monitor = ProcessMonitor.FromConfiguration(monitorConfiguration);
                    _monitors.Add(monitor);
                }
                catch (Exception ex)
                {
                    LoggerHelper.Debug("AppsMonitorService::SetupMonitors({0}): Error - {1}", monitorConfiguration.Key, ex);
                }
            }
        }

        private void SetupRequestEvents()
        {
            foreach (var monitor in _monitors)
            {
                GetManager().SubscribeTopic(this, GetSubscribeTopic(monitor), OnSetStateMessage);
            }
        }

        private void CheckAllMonitors(bool forceSendState = false)
        {
            StopTimers();

            foreach (ProcessMonitor monitor in _monitors)
            {
                try
                {
                    var monitor1 = monitor;
                    CheckMonitor(monitor1, forceSendState);
                }
                catch (Exception ex)
                {
                    LoggerHelper.Error("AppsMonitorService::ExecuteAllMonitors({0}) - Exception: {1}", monitor.DisplayName, ex);
                }
            }

            RestartTimers();
        }

        private void CheckMonitor(ProcessMonitor monitor, bool forceSendState = false)
        {
            var isUpdated = ProcessHelper.SetProcessState(ref monitor);

            if (isUpdated || forceSendState)
                SendMonitorValue(GetStateTopic(monitor), StateToString(monitor.State));
        }

        private void SendMonitorValue(string topic, string value)
        {
            if (string.IsNullOrWhiteSpace(topic))
                return;

            GetManager().PublishMessage(this, topic, value);
        }

        private string StateToString(ProcessState state)
        {
            switch (state)
            {
                case ProcessState.Running:
                    return PAYLOAD_ON;
                case ProcessState.NotRunning:
                    return PAYLOAD_OFF;
                default:
                    return PAYLOAD_OFF;
            }
        }

        private string GetStateTopic(ProcessMonitor monitor)
        {
            return $"apps/{monitor.DisplayName}/state";
        }

        private string GetSubscribeTopic(ProcessMonitor monitor)
        {
            return $"apps/{monitor.DisplayName.ToLower()}/set";
        }

        private void OnSetStateMessage(object sender, MQTTMessageEventEventArgs e)
        {
            string value = e.Message.GetPayload();
            string topic = e.Message.Topic;

            if (value == null)
                return;

            foreach (var monitor in _monitors)
            {
                if (topic.Replace("apps-monitor/", "") == GetSubscribeTopic(monitor))
                {
                    switch (value)
                    {
                        case "ON":
                            ProcessHelper.StartProcess(monitor);
                            break;
                        case "OFF":
                            ProcessHelper.KillProcess(monitor);
                            break;
                    }
                    CheckMonitor(monitor);
                }
            }
        }

        // Timer
        private void OnMonitorTimerElapsed(object source, ElapsedEventArgs e) => CheckAllMonitors();

        private void CleanTimers()
        {
            if (_monitorTimer != null)
            {
                _monitorTimer.Stop();
                _monitorTimer = null;
            }
        }

        private void StartTimers()
        {
            if (!IsAddonEnabled())
                return;

            CleanTimers();

            _monitorTimer = new Timer();
            _monitorTimer.Elapsed += new ElapsedEventHandler(OnMonitorTimerElapsed);
            _monitorTimer.Interval = _timerInterval;
            _monitorTimer.Start();
        }

        private void RestartTimers()
        {
            if (_monitorTimer == null)
                return;

            _monitorTimer.Stop();
            _monitorTimer.Start();
        }

        private void StopTimers()
        {
            if (_monitorTimer == null)
                return;

            _monitorTimer.Stop();
        }
    }
}
