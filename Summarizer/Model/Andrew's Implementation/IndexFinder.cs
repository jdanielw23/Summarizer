using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    class IndexFinder
    {
        private string[] source;

        public IndexFinder(string[] source)
        {
            this.source = source;
        }

        public int Index(string item)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == item)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
