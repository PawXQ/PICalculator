using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICalculator.Utility
{
    internal class PiCalculator
    {
        static Random random = new Random(Guid.NewGuid().GetHashCode());
        public static double Calculate(long sample)
        {
            int sum = 0;
            for (int i = 0; i < sample; i++)
            {
                //if (Math.Pow(random.NextDouble(), 2) + Math.Pow(random.NextDouble(), 2) < 1)
                //{
                //    sum++;
                //}

                double x = random.NextDouble();
                double y = random.NextDouble();

                if (x * x + y * y < 1.0)
                {
                    sum++;
                }
            }
            return 4.0 * sum / (sample);
        }
    }
}
