using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    class Monitor
    {
        private int op_size;
        private int progress;
        private int percentage;

        public Monitor(string name, int size)
        {
            op_size = size;
            Console.WriteLine("STARTING " + name.ToUpper());
            progress = 0;
            percentage = 0;
            notify();
        }

        public void Ping()
        {
            progress++;
            int new_percentage = (100 * progress) / op_size;
            if (new_percentage == percentage + 20)
            {
                percentage = new_percentage;
                notify();
            }
        }

        private void notify()
        {
            Console.WriteLine("*** " + percentage + "% COMPLETE   ");
        }
    }
}
