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
            test.TestAllFile(150000, 0.2, 0.2);
            /*
            var dumdum = new DummyClass("jan","mak",99);
            var fh = new FIleHandler("dumdum",dumdum.Size());
            fh.InsertToFile(dumdum.ToByteArray());
            fh.InsertToFile(dumdum.ToByteArray());
            System.Console.WriteLine(dumdum.FromByteArray(fh.ReadFromFile(0)).ToString());
            System.Console.WriteLine(dumdum.FromByteArray(fh.ReadFromFile(1)).ToString());
            System.Console.WriteLine(dumdum.FromByteArray(fh.ReadFromFile(2)).ToString());
            System.Console.WriteLine(dumdum.FromByteArray(fh.ReadFromFile(3)).ToString());
            System.Console.WriteLine(dumdum.FromByteArray(fh.ReadFromFile(4)).ToString());*/
            //System.Console.WriteLine("DONE");
        }
    }
}