using PICalculator.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PICalculator.Contract.TaskContract;

namespace PICalculator.Presenter
{
    internal class TaskPresenter : ITaskPresenter
    {
        ITaskView tasksView;
        Stopwatch sw = new Stopwatch();
        public TaskPresenter(ITaskView tasksView)
        {
            this.tasksView = tasksView;
        }

        //public void AddTask(long sample)
        //{
        //    sw.Start();
        //    double result = PiCalculator.Calculate(sample);
        //    sw.Stop();
        //    double swTotal = sw.ElapsedMilliseconds;

        //    PiTask task = new PiTask(sample, swTotal.ToString(), result);

        //    this.tasksView.RenderTask(task);
        //}

        public void AddTask(long sample)
        {
            Task.Run(() =>
            {
                sw.Start();
                double result = PiCalculator.Calculate(sample);
                sw.Stop();
                double swTotal = sw.ElapsedMilliseconds;

                PiTask task = new PiTask(sample, swTotal.ToString(), result);

                this.tasksView.RenderTask(task);
            });
        }

        //public async Task AddTask(long sample)
        //{
        //    await Task.Run(() =>
        //    {
        //        sw.Start();
        //        double result = PiCalculator.Calculate(sample);
        //        sw.Stop();
        //        double swTotal = sw.ElapsedMilliseconds;

        //        PiTask task = new PiTask(sample, swTotal.ToString(), result);

        //        this.tasksView.RenderTask(task);
        //    });
        //}

        //public Task AddTask(long sample)
        //{
        //    return Task.Run(() =>
        //    {
        //        sw.Start();
        //        double result = PiCalculator.Calculate(sample);
        //        sw.Stop();
        //        double swTotal = sw.ElapsedMilliseconds;

        //        PiTask task = new PiTask(sample, swTotal.ToString(), result);

        //        this.tasksView.RenderTask(task);
        //    });
        //}
    }
}
