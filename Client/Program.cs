using Memcached.Mimic.Commands;
using Memcached.Mimic.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            bool portParsed = false;
            int port = -1;
            while (!portParsed)
            {
                Console.WriteLine("Please, define the port number of the server you wanna connect to");
                portParsed = int.TryParse(Console.ReadLine(), out port);
            }

            bool clientModeParsed = false;
            string modeInput = "";
            while (!clientModeParsed)
            {
                Console.WriteLine("Do you wanna manually send commands (yes/no)");
                modeInput = Console.ReadLine();
                clientModeParsed = (modeInput == "yes" || modeInput == "no");
            }

            bool numberOfClientsParsed = false;
            int numberOfClients = 0;

            if (modeInput == "yes")
                new Memcached.Mimic.Client.Client("127.0.0.1", port);
            else while (!numberOfClientsParsed)
                {
                    Console.WriteLine("How many clients do you wanna launch ?");
                    numberOfClientsParsed = int.TryParse(Console.ReadLine(), out numberOfClients);
                    numberOfClientsParsed = numberOfClientsParsed && numberOfClients > 0;
                }

            List<Memcached.Mimic.Client.Client> clients = new List<Memcached.Mimic.Client.Client>();
            for (int i = 0; i < numberOfClients; i++)
            {
                Memcached.Mimic.Client.Client client
                    = new Memcached.Mimic.Client.Client("127.0.0.1", port, false);
                clients.Add(client);
            }

            foreach (var client in clients)
            {
                var keyLength = new Random().Next(1, (int)Math.Pow(2, 14));
                var keyValueLength = new Random().Next(1, (int)Math.Pow(2, 14));

                var randomKey = RandomGenerator.GenerateRandomKey(keyLength);
                var randomValue = RandomGenerator.GenerateRandomKey(keyValueLength);
                client.SendCommand(new SetCommand(randomKey, keyValueLength, randomValue));
                Task.Delay(200).Wait();
            }
        }
    }
}
