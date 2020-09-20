using Memcached.Mimic.Commands;
using Memcached.Mimic.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Memcached.Mimic.Tests
{
    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void GetCommandParses_UserInput()
        {
            GetCommand command = CommandParser.ParseFromUserInput("      get a", false) as GetCommand;
            Assert.AreEqual("a", command.Key);

            command = CommandParser.ParseFromUserInput("      get otherKey", false) as GetCommand;
            Assert.AreEqual("otherKey", command.Key);

            command = CommandParser.ParseFromUserInput("      gget other___Key", false) as GetCommand;
            Assert.AreEqual(command, null);
        }
        [TestMethod]
        public void GetCommandParses_Sent()
        {
            GetCommand command = CommandParser.ParseFromSentData("      get a") as GetCommand;
            Assert.AreEqual("a", command.Key);
            command = CommandParser.ParseFromSentData("      get otherKey") as GetCommand;
            Assert.AreEqual("otherKey", command.Key);
            command = CommandParser.ParseFromSentData("      gget other Key") as GetCommand;
            Assert.AreEqual(command, null);
        }


        [TestMethod]
        public void SetCommandParses_UserInput()
        {
            SetCommand command = CommandParser.ParseFromUserInput("      set myKey 1024", false, "someKeyValue") as SetCommand;
            Assert.AreEqual("myKey", command.Key);
            Assert.AreEqual(1024, command.ValueSize);
            Assert.AreEqual("someKeyValue", command.Value);
        }
        [TestMethod]
        public void SetCommandParses_Sent()
        {
            SetCommand command = CommandParser.ParseFromSentData("      set myKey 1024 someKeyValue") as SetCommand;
            Assert.AreEqual("myKey", command.Key);
            Assert.AreEqual(1024, command.ValueSize);
            Assert.AreEqual("someKeyValue", command.Value);


        }


        [TestMethod]
        public void DeleteCommandParses_UserInput()
        {
            DeleteCommand command = CommandParser.ParseFromUserInput("      delete a", false) as DeleteCommand;
            Assert.AreEqual("a", command.Key);

            command = CommandParser.ParseFromUserInput("      delete otherKey", false) as DeleteCommand;
            Assert.AreEqual("otherKey", command.Key);

            command = CommandParser.ParseFromUserInput("      ddelete other___Key", false) as DeleteCommand;
            Assert.AreEqual(command, null);
        }
        [TestMethod]
        public void DeleteCommandParses_Sent()
        {
            DeleteCommand command = CommandParser.ParseFromSentData("      delete a") as DeleteCommand;
            Assert.AreEqual("a", command.Key);
            command = CommandParser.ParseFromSentData("      delete otherKey") as DeleteCommand;
            Assert.AreEqual("otherKey", command.Key);
            command = CommandParser.ParseFromSentData("      gget other Key") as DeleteCommand;
            Assert.AreEqual(command, null);
        }
    }
}
