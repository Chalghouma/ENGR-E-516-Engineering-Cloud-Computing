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
            GetCommand command = CommandParser.ParseFromUserInput("      get a",false) as GetCommand;
            Assert.AreEqual("a", command.Key);

            command = CommandParser.ParseFromUserInput("      get otherKey", false) as GetCommand;
            Assert.AreEqual("otherKey", command.Key);

            command = CommandParser.ParseFromUserInput("      gget other Key", false) as GetCommand;
            Assert.AreEqual(command, null);
        }
        [TestMethod]
        public void SetCommandParses()
        {
            SetCommand command = CommandParser.ParseFromUserInput("      set myKey 1024", false,"someKeyValue") as SetCommand;
            Assert.AreEqual("myKey", command.Key);
            Assert.AreEqual(1024, command.ValueSize);
            Assert.AreEqual("someKeyValue", command.Value);
        }
    }
}
