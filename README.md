# IoTLinkAddons
[![GitHub license](https://img.shields.io/github/license/DragonNP/IoTLinkAddons)](https://github.com/DragonNP/IoTLinkAddons/blob/master/LICENSE)

IoTLinkAddons is free addons for [IoTLink](https://iotlink.gitlab.io/)

## Monitor Addon
### Features
CPU:
 - Name
 - Clock cores (only up to 4 cores)
 - Bus Speed
 - Temperatures (up to 4 cores, package, max, average)
 - Load (up to 4 cores, total)
 - Powers (package, cores, graphics, memory, all)

### Configuration
Configuration file: %ProgramData%\IOTLink\Addons\Monitor\addon.yaml
``` yaml
# CPU configuration
cpu_name: true   # Send cpu name to mqtt (sent only once)
cpu_clocks: true # Send cpu clocks to mqtt
cpu_temps: true  # Send cpu temperature to mqtt
cpu_load: true   # Send cpu load to mqtt
cpu_powers: true # Send cpu powers to mqtt
```

### Libraries:
 - [LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor)
