using Memcached.Mimic.FileHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Memcached.Mimic.Tests
{
    [TestClass]
    public class TextFileHandlerTests
    {
        const string FilePath = @"Test.DB.txt";
        [TestMethod]
        public void CheckFileExists()
        {
            var handler = new TextFileHandler(FilePath);
            Assert.IsTrue(File.Exists(FilePath));
        }
        [TestMethod]
        void Cleanup(string path = FilePath)
        {
            File.Delete(path);
        }
        [TestMethod]
        public void GetKeyFromEmptyFile()
        {
            Cleanup();
            var handler = new TextFileHandler(FilePath);
            string keyValue = "";
            Assert.IsFalse(handler.GetKeyValue("SomeKey", out keyValue));
        }
        [TestMethod]
        public void CanStoreKey()
        {
            Cleanup();
            var handler = new TextFileHandler(FilePath);
            Assert.IsTrue(handler.SetKey("SomeKey", "SomeValue"));
        }
        [TestMethod]
        public void StoreGet10Keys()
        {
            Cleanup();
            var handler = new TextFileHandler(FilePath);
            int count = 10;
            string keyValue = "";
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(handler.SetKey($"Key{i}", $"Value{i}"));
                //We check twice, right after an insertion is made, and when all the set is done
                Assert.IsTrue(handler.GetKeyValue($"Key{i}", out keyValue));
                Assert.AreEqual($"Value{i}", keyValue);
            }
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(handler.GetKeyValue($"Key{i}", out keyValue));
                Assert.AreEqual($"Value{i}", keyValue);
            }
        }
    }
}
