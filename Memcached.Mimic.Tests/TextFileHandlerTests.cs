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
        const string FilePath = @"C:\Users\cheri\OneDrive\Documents\MSDS"
            + @"\Engineering Cloud Computing\Assignements\Memcached.Mimic\Memcached.Mimic.Tests\DB.txt";
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
            Assert.IsNull(handler.GetKeyValue("SomeKey"));
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
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(handler.SetKey($"Key{i}", $"Value{i}"));
            }
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual($"Value{i}", handler.GetKeyValue($"Key{i}"));
            }
        }
    }
}
