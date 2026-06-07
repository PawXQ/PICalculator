using PICalculator.Utility;
using System;
using System.Collections.Concurrent;
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

        public ConcurrentDictionary<long, bool> PiTasksStatus = new ConcurrentDictionary<long, bool>();
        private ConcurrentQueue<long> PiTaskSampleQueue = new ConcurrentQueue<long>();

        Stopwatch sw = new Stopwatch();
        public TaskPresenter(ITaskView tasksView)
        {
            this.tasksView = tasksView;
            RunTask();
        }

        public void AddTask(long sample)
        {
            Task.Run(() =>
            {
                bool initialStatus = InitialTaskStatus(sample);

                if (initialStatus)
                {
                    this.PiTaskSampleQueue.Enqueue(sample);
                }
            });
        }

        public void RunTask()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    long PiTaskSample = 0;

                    if (this.PiTaskSampleQueue.TryDequeue(out long result)) { PiTaskSample = result; }
                    else { continue; }

                    PiTask task = new PiTask(PiTaskSample);

                    this.tasksView.RenderTask(task);

                    Task.Run(() =>
                    {
                        sw.Start();
                        double piResult = PiCalculator.Calculate(PiTaskSample);
                        sw.Stop();
                        double swTotal = sw.ElapsedMilliseconds;

                        task.Time = swTotal.ToString();
                        task.Value = piResult;

                        this.tasksView.RenderTask(task);

                        CompleteTaskStatus(PiTaskSample);
                    });
                }
            });
        }

        private bool InitialTaskStatus(long sample)
        {
            if (this.PiTasksStatus.ContainsKey(sample))
            {
                Console.WriteLine($"{sample} sample duplicate");
                return false;
            }
            this.PiTasksStatus[sample] = false;
            return true;
        }

        private void CompleteTaskStatus(long sample)
        {
            this.PiTasksStatus[sample] = true;
        }
    }
}
