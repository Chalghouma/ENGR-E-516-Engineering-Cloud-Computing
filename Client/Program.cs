using System;

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
            new Memcached.Mimic.Client.Client("127.0.0.1", port);
        }
    }
}
