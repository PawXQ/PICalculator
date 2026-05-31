using PICalculator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PICalculator.Contract.TaskContract;

namespace PICalculator.Presenter
{
    internal class TaskPresenter : ITaskPresenter
    {
        ITaskView tasksView;
        public TaskPresenter(ITaskView tasksView)
        {
            this.tasksView = tasksView;
        }
        public void AddTask(long sample)
        {
            // calculate return task

            //string result = PiCalculator.Calculate(sample);

            Task task = new Task(sample, "123", 3.14);

            this.tasksView.RenderTask(task);
        }
    }
}
