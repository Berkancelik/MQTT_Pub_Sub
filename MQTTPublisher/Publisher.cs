using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Threading.Tasks;

namespace MQTTPublisher
{
    internal class Publisher
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
            client.UseConnectedHandler(e =>
            {
                Console.WriteLine("Connected to the broker successfull");
            });

            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected frım the broker successfully");
            });

            await client.ConnectAsync(options);

            Console.WriteLine("Please press a key to publish the message");

            Console.ReadLine();
            await PublihsMessageAsync(client);

            await client.DisconnectAsync();
        }

            private  static async Task PublihsMessageAsync(IMqttClient client)
            {
                string messagePayload = "Hello!";
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("Berkancelik")
                    .WithPayload(messagePayload)
                    .WithAtLeastOnceQoS()
                    .Build();

                if (client.IsConnected)
                {
                    await client.PublishAsync(message);
                }
                
            }

        }
    }
}
