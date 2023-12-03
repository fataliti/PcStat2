using OpenHardwareMonitor.Hardware;

namespace PcStat2
{   
    class HardwareMonitor
    {
        Computer        computer;
        HardwareValues  hardware_values;

        public HardwareMonitor()
        {
            computer = new Computer
            {
                GPUEnabled = true,
                CPUEnabled = true,
                RAMEnabled = true, 
            };
            hardware_values = new HardwareValues();
            computer.Open();
        }

        public void CollectData()
        {
            foreach (var hardware in computer.Hardware)
            {
                hardware.Update();

                switch (hardware.HardwareType)
                {
                    case HardwareType.CPU:
                        foreach (var sensor in hardware.Sensors)
                        {
                            switch (sensor.SensorType)
                            {
                                case SensorType.Temperature:
                                    if (sensor.Name.Contains("CPU Package"))
                                    {
                                        hardware_values.CpuTemp = sensor.Value.GetValueOrDefault();
                                    }
                                    break;

                                case SensorType.Load:
                                    if (sensor.Name.Contains("CPU Total"))
                                    {
                                        hardware_values.CpuLoad = sensor.Value.GetValueOrDefault();
                                    }
                                    break;
                            }
                        }
                        break;

                    case HardwareType.GpuNvidia:
                    case HardwareType.GpuAti:
                        foreach (var sensor in hardware.Sensors)
                        {
                            switch (sensor.SensorType)
                            {
                                case SensorType.Temperature:
                                    hardware_values.GpuTemp = sensor.Value.GetValueOrDefault();
                                    break;
                            
                                case SensorType.Load:
                                    hardware_values.GpuLoad = sensor.Value.GetValueOrDefault();
                                    break;
                            }
                        }   
                        break;

                    case HardwareType.RAM:
                        foreach (var sensor in hardware.Sensors)
                        {
                            switch (sensor.SensorType)
                            {
                                case SensorType.Load:
                                    hardware_values.MemLoad = sensor.Value.GetValueOrDefault();
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        public string GetSerializedData()
        {
            return hardware_values.GetSerializedData();
        }
    }
}
