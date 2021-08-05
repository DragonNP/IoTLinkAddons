# IoTLinkAddons
[![GitHub license](https://img.shields.io/github/license/DragonNP/IoTLinkAddons)](https://github.com/DragonNP/IoTLinkAddons/blob/master/LICENSE)

IoTLinkAddons is free addons for [IoTLink](https://iotlink.gitlab.io/)

## Monitor Addon
### Features
CPU:
 - Name
 - Clocks
 - Temperatures
 - Powers
GPU Nvidia:
 - Name
 - Clocks
 - Temperatures
 - Load
 - Controls
 - Data
Memory:
 - Data
 - Load
Storages:
 - Temperature
 - Used space

### Configuration
Configuration file: %ProgramData%\IOTLink\Addons\Monitor\addon.yaml
``` yaml
# Configuration
cpu: true
memory: true
gpu_nvidia: true
storages: true
```

### Libraries:
 - [LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor)

## Apps Monitor
### Features
- Monitoring state app
- Close and open apps from mqtt topic
### Configuration
Configuration file: %ProgramData%\IOTLink\Addons\AppsMonitor\addon.yaml
``` yaml
# Configuration
monitors:
  Steam:
    process_name: steam                  // The process to follow
    path_to_exe: E:\Game\Steam\steam.exe // Run exe
    display_name: Steam                 // show in mqtt topic
  Edge:
    process_name: msedge
    path_to_exe: C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe
    display_name: Edge
```
```
# MQTT Topics
../apps-monitor/apps/{display_name}/state // State app
../apps-monitor/apps/{display_name}/set   // App control ON -> open app; OFF -> close app
```
