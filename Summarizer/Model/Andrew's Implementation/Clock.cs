using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Summarizer.Model.Andrew_s_Implementation
{
    public class Clock
    {
        private Stopwatch sw;

        public Clock()
        {
            sw = new Stopwatch();
            sw.Start();
        }

        public string query()
        {
            sw.Stop();
            return "Time elapsed was " + sw.Elapsed + ".";
        }
    }
}
