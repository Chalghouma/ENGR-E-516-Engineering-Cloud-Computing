using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            new Memcached.Mimic.Client.Client("127.0.0.1",10001);
        }
    }
}
