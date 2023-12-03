using Newtonsoft.Json;

namespace PcStat2
{
    internal class HardwareValues
    {
        public double CpuTemp;
        public double GpuTemp;
        public double CpuLoad;
        public double GpuLoad;
        public double MemLoad;

        public string GetSerializedData()
        {
            var dataObject = new
            {
                CpuTemp = CpuTemp,
                GpuTemp = GpuTemp,
                CpuLoad = CpuLoad,
                GpuLoad = GpuLoad,
                MemLoad = MemLoad
            };

            return JsonConvert.SerializeObject(dataObject);
        }
    }
}
