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
        public void AddTask(long sample)
        {
            sw.Start();
            double result = PiCalculator.Calculate(sample);
            sw.Stop();
            double swTotal = sw.ElapsedMilliseconds;

            Task task = new Task(sample, swTotal.ToString(), result);

            this.tasksView.RenderTask(task);
        }
    }
}
