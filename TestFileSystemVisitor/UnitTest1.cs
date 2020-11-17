using System;
using NUnit.Framework;
using FileSystemVisitorProject;

namespace TestFileSystemVisitor
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            var fsVisitor = new FileSystemVisitor(@"C:\Users\Amrah\Downloads\task2_advanced_c#\");
            Assert.AreNotEqual(null, fsVisitor.ListDirectories());
        }
    }
}
