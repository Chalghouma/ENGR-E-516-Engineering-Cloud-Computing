using Memcached.Mimic.Commands;
using Memcached.Mimic.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MainServer = Memcached.Mimic.Server.Server;
namespace Memcached.Mimic.Tests
{
    [TestClass]
    public class ServerSocketTests
    {
        [TestMethod]
        public void ExtensiveStoreGet10KeysThenDeleteOne()
        {
            MainServer server = null;
            Task.Run(() =>
            {
                server = new MainServer("127.0.0.1", 10001);
                server.ListenForClients();
            });

            var client = new Client.Client("127.0.0.1", 10001, false);
            for (int i = 0; i < 16; i++)
            {
                int pow = (int)Math.Pow(2, i);
                var key = RandomGenerator.GenerateRandomKey(pow);
                var value = RandomGenerator.GenerateRandomKey(pow);
                client.SendCommand(new SetCommand(key, pow, value));
                Task.Delay(100).Wait();
                string storedKeyValue;
                server.FileHandler.GetKeyValue(key, out storedKeyValue);
                Assert.AreEqual(value, storedKeyValue);
            }
        }
    }
}
