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

### Configuration
Configuration file: %ProgramData%\IOTLink\Addons\Monitor\addon.yaml
``` yaml
# CPU configuration
cpu_name: true   # Send cpu name to mqtt (sent only once)
cpu_clocks: true # Send cpu clocks to mqtt
cpu_temps: true  # Send cpu temperature to mqtt
cpu_powers: true # Send cpu powers to mqtt
```

### Libraries:
 - [LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor)
