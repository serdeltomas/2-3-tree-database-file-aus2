using System;

namespace Semka1
{
    public class Start
    {
        
        static void Main()
        {
            var test = new Test();
            //var app = new App();
            //app.GenerateData(1000000, 1000000);
            test.TestAll(200000,0.2,0.2);
            //System.Console.WriteLine("DONE");
        }
    }
}