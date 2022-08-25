using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet;
using System;
using System.Threading.Tasks;
using System.Text;

namespace MQTTSubscriber
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            var client = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("test.mosquitto.org", 1883)
                .WithCleanSession()
                .Build();
            client.UseConnectedHandler(async e =>
            {
                Console.WriteLine("Connected to the broker successfull");
                var topicFilter = new TopicFilterBuilder()
                .WithTopic("Berkancelik")
                .Build();
               await client.SubscribeAsync(topicFilter);
            });

            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected frım the broker successfully");
            });
            client.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine($"Received Message - {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            });

            await client.ConnectAsync(options);
            Console.ReadLine();
            await client.DisconnectAsync();

          
        

        
        }
    }
}
