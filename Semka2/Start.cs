using System;

namespace Semka2
{
    public class Start
    {
        
        static void Main()
        {
            //var app = new App();
            //app.GenerateData(1000000, 1000000);
            var test = new Test();
            test.TestAll(200000,0.2,0.2);
            //System.Console.WriteLine("DONE");
        }
    }
}