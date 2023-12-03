using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

namespace PcStat2
{
    class HardwareMqttClient
    {
        IMqttClient mqttClient;

        string server = "hivemq.cloud";
        int port = 8883;
        string login = "";
        string password = "";
        string client_id = "";
        string topic = "";

        public HardwareMqttClient()
        {
            var factory = new MqttFactory();
            mqttClient  = factory.CreateMqttClient();

        }

        public async Task Connect()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(server, port)
                .WithCredentials(login, password)
                .WithClientId(client_id)
                .WithTls()
                .Build();

            var response = await mqttClient.ConnectAsync(options);

            //Console.WriteLine("The MQTT client is connected.");
        }

        public async Task SendData(string _data)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(_data)
                .Build();

            await mqttClient.PublishAsync(message);
        }
    }
}
