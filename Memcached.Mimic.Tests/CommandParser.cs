using Memcached.Mimic.Commands;
using Memcached.Mimic.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Memcached.Mimic.Tests
{
    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void GetCommandParses()
        {
            GetCommand command = CommandParser.ParseFromUserInput("      get a") as GetCommand;
            Assert.AreEqual("a", command.Key);

            command = CommandParser.ParseFromUserInput("      get otherKey") as GetCommand;
            Assert.AreEqual("otherKey", command.Key);

            command = CommandParser.ParseFromUserInput("      gget other Key") as GetCommand;
            Assert.AreEqual(command, null);
        }
    }
}
