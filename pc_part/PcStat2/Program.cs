using System.Threading;
using System.Threading.Tasks;

namespace PcStat2
{
    class Program
    {
        static HardwareMonitor monitor = new HardwareMonitor();
        static HardwareMqttClient hardwareMqttClient = new HardwareMqttClient();

        static int update_per_ms = 5000;


        static async Task Main(string[] args)
        {
            await hardwareMqttClient.Connect();

            while (true)
            {
                Thread.Sleep(update_per_ms);
                monitor.CollectData();
                //System.Diagnostics.Debug.WriteLine(monitor.GetSerializedData());
                await hardwareMqttClient.SendData(monitor.GetSerializedData());
            }
        }
    }
}