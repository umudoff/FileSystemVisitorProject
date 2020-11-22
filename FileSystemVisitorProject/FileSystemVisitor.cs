using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitorProject
{
     
    public class FileSystemVisitor
    {
        private string path;

        private Func<string, bool> filterLogic;


        public class FindedEventArgs: EventArgs
        {
            public string FindedName { get; set; }
        }

        public class FilteredFindedEventArgs : EventArgs
        {
            public string FilteredFindedName { get; set; }
            public bool StopSearch;
            public bool ExcludeFinded;
        }


        public event EventHandler StartEvent;
        public event EventHandler FinishEvent;
        public event EventHandler<FindedEventArgs> FileFindedEvent;
        public event EventHandler<FindedEventArgs> DirectoryFindedEvent;
        public event EventHandler<FilteredFindedEventArgs> FilteredFileFindedEvent;
        public event EventHandler<FilteredFindedEventArgs> FilteredDirectoryFindedEvent;

        public FileSystemVisitor(string path)
        {
            this.path = path;
        }

        public FileSystemVisitor(string path, Func<string, bool> filterLogic)
        {
            this.path = path;
            this.filterLogic = filterLogic;
        }




        public IEnumerable<String> ListDirectories()
        {
            StartEvent?.Invoke(this, new EventArgs());
            var listDirs = new List<string>(Directory.GetFileSystemEntries(path,"*.*", SearchOption.AllDirectories));
           
            foreach (var file in listDirs)
            {


                if (Directory.Exists(file))
                {
                    DirectoryFindedEvent?.Invoke(this, new FindedEventArgs { FindedName = file });
                }
                else
                {
                    FileFindedEvent?.Invoke(this, new FindedEventArgs { FindedName= file});
                }

                if (filterLogic!=null)
                {
                    if (filterLogic(file))
                    {
                        if (Directory.Exists(file))
                        {
                            var fArgs = new FilteredFindedEventArgs { FilteredFindedName = file };
                            FilteredDirectoryFindedEvent?.Invoke(this, fArgs);
                            if (fArgs.StopSearch) yield break;
                            if (fArgs.ExcludeFinded) continue;
                        }

                        if (File.Exists(file))
                        {
                            var fArgs = new FilteredFindedEventArgs { FilteredFindedName = file };
                            FilteredFileFindedEvent?.Invoke(this, fArgs);
                            if (fArgs.ExcludeFinded) continue;
                            if (fArgs.StopSearch) yield break;
                           
                        }

                    }

                }

                
                yield return file;
            }

            FinishEvent?.Invoke(this, new EventArgs());
        }

    }
}
