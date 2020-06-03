using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreoleForth
{
    public class AppSpec
    {
        string title = "Application-specific grouping";

        public AppSpec()
        {

        }

        public string Title
        {
            get { return title; }
        }

        public void doTest(GlobalSimpleProps gsp)
        {
            Console.WriteLine("Testing definition - do whatever you want here");
        }


    }
}
