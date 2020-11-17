using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitorProject
{
    class Program
    {
        static void Main(string[] args)
        {
         
             FileSystemVisitor fileVisitor = new FileSystemVisitor(@"C:\Users\Amrah\Downloads\task2_advanced_c#\02. Advanced CSharp",(x) =>  { return x.Contains(".txt"); });
             fileVisitor.StartEvent += (object o, EventArgs  e) => {
                 Console.WriteLine(" FileVisitor started");
             };
            fileVisitor.FinishEvent += (object o, EventArgs e) =>
            {
                Console.WriteLine("FileVisitor finished");
            };
            fileVisitor.DirectoryFindedEvent += (o, e) => {
                Console.WriteLine("Directory Found: " +e.FindedName);
            };
            fileVisitor.FileFindedEvent += (o,e) =>{
                Console.WriteLine("File Found: " + e.FindedName);
            };
            fileVisitor.FilteredDirectoryFindedEvent += (o, e) =>
            {
                Console.WriteLine("!!!Filtered Directory Found:" + e.FilteredFindedName);

            };

            fileVisitor.FilteredFileFindedEvent += (o, e) =>
            {
               
               if (e.FilteredFindedName.Contains("dummy")) e.StopSearch = true;
                if (e.FilteredFindedName.Contains(".pptx")) { 
                    e.ExcludeFinded = true;
                    Console.WriteLine("Excluded file: "+e.FilteredFindedName);
                }
                Console.WriteLine("!!!Filtered File Found:" + e.FilteredFindedName);
            };
 

           foreach(var file in fileVisitor.ListDirectories())
            {
              // Console.WriteLine(file);
            }
              

            Console.ReadLine();


        }
    }
}
