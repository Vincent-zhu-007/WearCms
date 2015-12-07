using System;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MobileCms.Common
{
    public class MqttHelp
    {
        public static void SendMqttMessage(string topic, string data)
        {
            string mqttAddress = ConfigHelp.GetConfigString("MqttAddress");

            // create client instance 
            MqttClient client = new MqttClient(IPAddress.Parse(mqttAddress));

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // publish a message on "/home/temperature" topic with QoS 2 
            client.Publish(topic, Encoding.UTF8.GetBytes(data), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }
}
