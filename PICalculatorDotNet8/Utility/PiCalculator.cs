using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICalculatorDotNet8.Utility
{
    class PiCalculator
    {
        const int BATCH_QUANTITY = 2_500_000;

        static object obj = new object();

        //static Random random = new Random(Guid.NewGuid().GetHashCode());

        public static async Task<double> Calculate(long sample)
        {
            long BATCH = sample % BATCH_QUANTITY == 0 ? sample / BATCH_QUANTITY : sample / BATCH_QUANTITY + 1;
            long remainder = sample % BATCH_QUANTITY;

            //long[] sumArray = new long[BATCH];
            //long totalSum = 0;

            long sum = 0;

            await Parallel.ForAsync(0, BATCH, (number, token) =>
            {
                //int sum = 0;

                long quantity = BATCH_QUANTITY;
                if (number + 1 == BATCH && remainder != 0) quantity = remainder;
                long subTotal = 0;

                for (int i = 0; i < quantity; i++)
                {

                    //if (Math.Pow(random.NextDouble(), 2) + Math.Pow(random.NextDouble(), 2) < 1)
                    //{
                    //    sum++;
                    //}


                    //double x = random.NextDouble();
                    //double y = random.NextDouble();

                    double x = Random.Shared.NextDouble();
                    double y = Random.Shared.NextDouble();
                    if (x * x + y * y < 1.0)
                    {
                        //sum++;
                        subTotal++;
                    }
                    //Debug.WriteLine($"number: {number}, subTotal: {subTotal}");
                }
                Interlocked.Add(ref sum, subTotal);
                //sumArray[number] = sum;

                return ValueTask.CompletedTask;
            });

            //for (int i = 0; i < sumArray.Length; i++) totalSum += sumArray[i];

            //return 4.0 * totalSum / (sample);
            return 4.0 * sum / (sample);
        }
    }
}
