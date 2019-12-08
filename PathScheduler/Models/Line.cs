using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathScheduler
{
    public class Line
    {
        public string type { get; set; }
        public List<List<double>> coordinates { get; set; }
    }
}
