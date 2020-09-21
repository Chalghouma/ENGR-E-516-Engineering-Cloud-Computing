using Memcached.Mimic.Client;
using Memcached.Mimic.Commands;
using Memcached.Mimic.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            bool portParsed = false;
            int port = -1;
            while (!portParsed)
            {
                Console.WriteLine("Please, define the port number of your server");
                portParsed = int.TryParse(Console.ReadLine(), out port);
            }
            new Memcached.Mimic.Server.Server("127.0.0.1", port).ListenForClients();
            //List<Client> clients = new List<Client>();
            //for (int i = 0; i < 100; i++)
            //{
            //    var client = new Client("127.0.0.1", 10001, false);
            //    clients.Add(client);
            //}
            //for (int i = 0; i < 100; i++)
            //{
            //    int keyLength = 1000;
            //    int keyValueLength = 8000;
            //    clients[i].SendCommand(new SetCommand(RandomGenerator.GenerateRandomKey(1000), keyValueLength, RandomGenerator.GenerateRandomKey(8000)));
            //}
            //Task.Delay(2000).Wait();
            //client.SendCommand(new GetCommand("myKey"));
            //client.SendCommand(new SetCommand("myKey", 3, "HOW"));
            //Task.Delay(2000).Wait();
            //client.SendCommand(new GetCommand("myKey"));
            //client.SendCommand(new GetCommand("myKey"));
            //Task.Delay(2000).Wait();
            //client.SendCommand(new GetCommand("myKey"));

            //new Memcached.Mimic.Client.Client("127.0.0.1", 10001, true);
        }
    }
}
