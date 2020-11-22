using System;
using NUnit.Framework;
using FileSystemVisitorProject;
using System.IO;
using System.Reflection;

namespace TestFileSystemVisitor
{
    [TestFixture]
    public class UnitTest1 { 


        private FileSystemVisitor FileVisitor;
        private FileSystemVisitor FilteredFileVisitor;
        private FileSystemVisitor FilteredDirectoryVisitor;
        private string ResultPath;

        [SetUp]
        public void TestInit()
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string resPath = Path.GetFullPath(Path.Combine(RunningPath, @"..\..\"));
            resPath += @"Resources";
            ResultPath = resPath;
            FileVisitor = new FileSystemVisitor(ResultPath);
            FilteredFileVisitor = new FileSystemVisitor(ResultPath, (x) => { return x.Contains(".txt"); });
            FilteredDirectoryVisitor = new FileSystemVisitor(ResultPath, (x) => { return x.EndsWith("d"); });

        }

        [Test]
        public void TestFileVisitorReturnList()
        {
            int ItemCount = 0;
            FileSystemVisitor fv = FileVisitor;
            foreach ( var e in fv.ListDirectories()) {
                ItemCount++;
            }

            Assert.AreEqual(11, ItemCount);
        }

        [Test]
        public void TestFileVisitor_StartEvent()
        {
            FileSystemVisitor fv = FileVisitor;
            bool StartEventCalled = false;
            
            fv.StartEvent += (object o, EventArgs e) => {
                StartEventCalled = true;
            };
            foreach (var e in fv.ListDirectories())
            { }
            Assert.AreEqual(true,StartEventCalled);
        }

        [Test]
        public void TestFileVisitor_FileFindedEvent()
        {
            int fileCount = 0;
            FileSystemVisitor fv = FileVisitor;
            fv.FileFindedEvent += (o, e) =>
             {
                 fileCount++;
             };
            foreach (var e in fv.ListDirectories())
            { }
            Assert.AreEqual(6,fileCount);


        }

        [Test]
        public void TestFileVisitor_DirectoryFindedEvent()
        {
            int directoryCount = 0;
            FileSystemVisitor fv = FileVisitor;
            fv.DirectoryFindedEvent += (o, e) =>
            {
                directoryCount++;
            };
            foreach (var e in fv.ListDirectories())
            { }
            Assert.AreEqual(5,directoryCount);
        }

        [Test]
        public void TestFileVisitor_FilteredTextFileFound()
        {
            int TextFileCount = 0;
            FileSystemVisitor fv = FilteredFileVisitor;
            fv.FilteredFileFindedEvent += (o, e) =>
            {
                TextFileCount++;
            };
            foreach (var e in fv.ListDirectories())
            { }
            Assert.AreEqual(5, TextFileCount);
        }
        [Test]
        public void TestFileVisitor_FilteredDirectoryFound()
        {
            bool directoryFound = false;
            FileSystemVisitor fv = FilteredDirectoryVisitor;
            fv.FilteredDirectoryFindedEvent += (o, e) =>
             {
                 directoryFound = true;
             };
            foreach (var e in fv.ListDirectories())
            { }
            Assert.That(directoryFound, Is.True);

        }

        [Test]
        public void TestFileVisitor_StopSearchEvent()
        {
            bool stopCriteriaFound = false;
            FileSystemVisitor fv = FilteredFileVisitor;
            fv.FilteredFileFindedEvent += (o, e) =>
            {
                if (e.FilteredFindedName.Contains("dummy"))
                {
                    e.StopSearch = true;
                    stopCriteriaFound = true;
                }
            };
            foreach (var e in fv.ListDirectories())
            { }
            Assert.That(stopCriteriaFound,Is.True);
        }

        [Test]
        public void TestFileVisitor_ExcludeFile()
        {
            int skippedFileCount = 0;
            FileSystemVisitor fv = new FileSystemVisitor(ResultPath, (x) => { return x.Length>0; }); ;
            fv.FilteredFileFindedEvent += (o, e) =>
            {
                if (e.FilteredFindedName.Contains(".pptx"))
                {
                    e.ExcludeFinded = true;
                    skippedFileCount++;
                }
            };
            foreach (var e in fv.ListDirectories())
            { }

            Assert.AreEqual(1,skippedFileCount);



        }



    }
}
