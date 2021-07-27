using IOTLinkAPI.Platform.HomeAssistant;

namespace Monitor
{
    static class DiscoveryOptions
    {
        public static HassDiscoveryOptions Clock()
        {
            return new HassDiscoveryOptions
            {
                Id = "Clock",
                Unit = "Mhz",
                Name = "Clock",
                Component = HomeAssistantComponent.Sensor,
                Icon = "mdi:clock"
            };
        }

        public static HassDiscoveryOptions Temperature()
        {
            return new HassDiscoveryOptions
            {
                Id = "Temperature",
                Unit = "°C",
                Name = "Temperature",
                Component = HomeAssistantComponent.Sensor,
                Icon = "mdi:thermometer"
            };
        }

        public static HassDiscoveryOptions Load()
        {
            return new HassDiscoveryOptions
            {
                Id = "Load",
                Unit = "%",
                Name = "Load",
                Component = HomeAssistantComponent.Sensor,
                Icon = "mdi:cpu-64-bit"
            };
        }

        public static HassDiscoveryOptions Power()
        {
            return new HassDiscoveryOptions
            {
                Id = "Power",
                Unit = "W",
                Name = "Power",
                Component = HomeAssistantComponent.Sensor,
                Icon = "mdi:power-plug"
            };
        }
    }
}
