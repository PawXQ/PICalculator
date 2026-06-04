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

        public Dictionary<long, bool> TasksStatus = new Dictionary<long, bool>();

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
            InitialTaskStatus(sample);

            PiTask task = new PiTask(sample);

            this.tasksView.RenderTask(task);

            Task.Run(() =>
            {
                sw.Start();
                double result = PiCalculator.Calculate(sample);
                sw.Stop();
                double swTotal = sw.ElapsedMilliseconds;

                task.Time = swTotal.ToString();
                task.Value = result;

                this.tasksView.RenderTask(task);

                CompleteTaskStatus(sample);
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

        private void InitialTaskStatus(long sample)
        {
            if (this.TasksStatus.ContainsKey(sample))
            {
                Console.WriteLine($"{sample} sample duplicate");
                return;
            }
            this.TasksStatus[sample] = false;
        }

        private void CompleteTaskStatus(long sample)
        {
            this.TasksStatus[sample] = true;
        }
    }
}
