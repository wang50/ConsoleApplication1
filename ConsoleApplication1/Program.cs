using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;

namespace ConsoleApplication1
{
    class Program
    {
        static string eventHubName = "wanderhub";
        static string connectionString = "Endpoint=sb://wanderhub-ns.servicebus.chinacloudapi.cn/;SharedAccessKeyName=ReceiveRule;SharedAccessKey=5WVrbt3P707PBF41owIbM0300u+jjGlT7gV35Baou0c=";
        static void Main(string[] args)
        {
            Console.WriteLine("Press Ctrl-C to stop the sender process");
            Console.WriteLine("Press Enter to start now");
            Console.ReadLine();
           // SendingRandomMessages();

            Console.WriteLine(gettoken());
            Console.ReadLine();
        }

        //生成sastoken 
        static string gettoken() {
            ServiceBusConnectionStringBuilder connectionString = new ServiceBusConnectionStringBuilder("Endpoint=sb://wanderhub-ns.servicebus.chinacloudapi.cn/;SharedAccessKeyName=ReceiveRule;SharedAccessKey=5WVrbt3P707PBF41owIbM0300u+jjGlT7gV35Baou0c=");

            string ServiceBusNamespace = connectionString.Endpoints.First().Host;
            string namespaceKeyName = connectionString.SharedAccessKeyName;
            string namespaceKey = connectionString.SharedAccessKey;

            // Create a token valid for 45mins
            string token = SharedAccessSignatureTokenProvider.GetSharedAccessSignature(namespaceKeyName, namespaceKey, ServiceBusNamespace, TimeSpan.FromDays(365));
            return token;
        }

        static void SendingRandomMessages()
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            while (true)
            {
                try
                {
                    var message = Guid.NewGuid().ToString();
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, message);
                    eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                    Console.ResetColor();
                }

                Thread.Sleep(200);
            }
        }
    }
}
