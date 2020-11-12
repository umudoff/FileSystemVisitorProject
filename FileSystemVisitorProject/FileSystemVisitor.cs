using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitorProject
{
    public delegate void StartDelegate();
    public delegate void FinishDelegate();
    public class FileSystemVisitor
    {
        public event StartDelegate StartEvent;
        public event FinishDelegate FinishEvent;

        public IEnumerable ListDirectories(string path)
        {
            StartEvent();

            var listDirs = new List<string>(Directory.GetDirectories(path));

            foreach (var file in listDirs)
            {
                yield return file;
            }

            FinishEvent();
        }

    }
}
