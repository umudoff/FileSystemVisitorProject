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
         
             FileSystemVisitor fileVisitor = new FileSystemVisitor();
             fileVisitor.StartEvent += () => {
                 Console.WriteLine(" FileVisitor started");
             };
            fileVisitor.FinishEvent += () =>
            {
                Console.WriteLine("FileVisitor finished");
            };

           foreach(var file in fileVisitor.ListDirectories(@"C:\Users\Amrah\Downloads\task2_advanced_c#\02. Advanced CSharp"))
            {
                Console.WriteLine(file);
            }
              

            Console.ReadLine();


        }
    }
}
