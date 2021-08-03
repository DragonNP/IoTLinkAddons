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
