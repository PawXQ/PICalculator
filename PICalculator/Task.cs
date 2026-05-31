using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICalculator
{
    internal class Task
    {
        public long Sample { get; set; }
        public string Time { get; set; }
        public double Value { get; set; }

        public Task(long Sample, string Time, double Value)
        {
            this.Sample = Sample;
            this.Time = Time;
            this.Value = Value;
        }
    }
}
