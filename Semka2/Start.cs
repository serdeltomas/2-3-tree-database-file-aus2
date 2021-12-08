using System;
using System.Text;

namespace Semka2
{
    public class Start
    {
        
        static void Main()
        {
            //var app = new App();
            //app.GenerateData(1000000, 1000000);

            var test = new Test();
            //test.TestAll(200000,0.2,0.2);
            //test.TestInsertFile(1000000);
            //test.TestAllFile(100000, 0.2, 0.2);
            //test.TestInsertTreeFile(100000);
            test.TestAllTreeFile(1000, 0.2, 0.2);

            /*var dumdum = new DummyClass("jan",0.9899, 0);
            var fh = new FIleHandler<DummyClass>("dumdum",dumdum.Size());
            fh.InsertToFile(dumdum);
            dumdum = new DummyClass("jozo", 5.0/12, int.MaxValue);
            fh.InsertToFile(dumdum);
            dumdum = new DummyClass("fero", 12, 12);
            fh.InsertToFile(dumdum);
            System.Console.WriteLine(fh.ReadFromFile(0).ToString());
            System.Console.WriteLine(fh.ReadFromFile(1).ToString());
            System.Console.WriteLine(fh.ReadFromFile(2).ToString());
            dumdum = new DummyClass("Tommy", 5.0 / 10, 1998);
            fh.UpdateInFIle(dumdum,1);
            System.Console.WriteLine(fh.ReadFromFile(0).ToString());
            System.Console.WriteLine(fh.ReadFromFile(1).ToString());
            System.Console.WriteLine(fh.ReadFromFile(2).ToString());*/
            System.Console.WriteLine();
            System.Console.WriteLine("DONE");
        }
    }
}