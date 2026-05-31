using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICalculator.Contract
{
    internal class TaskContract
    {
        internal interface ITaskPresenter
        {
            void AddTask(long sample);
        }
        internal interface ITaskView
        {
            void RenderTask(Task task);
        }
    }
}
