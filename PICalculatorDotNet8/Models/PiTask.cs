using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PICalculatorDotNet8.Models
{
    class PiTask
    {
        public long Sample { get; set; }
        public string Time { get; set; }
        public double Value { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public PiTask(long Sample, string Time, double Value)
        {
            this.Sample = Sample;
            this.Time = Time;
            this.Value = Value;
        }

        public PiTask(long Sample, CancellationTokenSource cancellationTokenSource)
        {
            this.Sample = Sample;
            this.CancellationTokenSource = cancellationTokenSource;
        }
    }
}
