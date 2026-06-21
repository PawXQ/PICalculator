using PICalculatorDotNet8.Models;
using PICalculatorDotNet8.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PICalculatorDotNet8.Contract.TaskContract;

namespace PICalculatorDotNet8.Presenter
{
    class TaskPresenter : ITaskPresenter
    {
        ITaskView tasksView;

        private ConcurrentDictionary<long, PiTask> PiTasksStatus = new ConcurrentDictionary<long, PiTask>();
        private ConcurrentQueue<long> PiTaskSampleQueue = new ConcurrentQueue<long>();
        private ConcurrentBag<PiTask> piTasks = new ConcurrentBag<PiTask>();
        private ConcurrentBag<PiTaskDTO> piTaskDTOs = new ConcurrentBag<PiTaskDTO>();

        private SemaphoreSlim startMissionSemaphoreSlim = new SemaphoreSlim(0);

        public TaskPresenter(ITaskView tasksView)
        {
            this.tasksView = tasksView;
            StartMission();
        }

        public void AddTask(long sample)
        {
            Task.Run(() =>
            {
                bool initialStatus = InitialTask(sample);

                if (initialStatus)
                {
                    this.PiTaskSampleQueue.Enqueue(sample);

                    PiTaskDTO piTaskDTO = new PiTaskDTO(sample);
                    this.tasksView.OnAddedRenderTask(piTaskDTO);

                    startMissionSemaphoreSlim.Release();
                }
            });
        }

        private bool InitialTask(long sample)
        {
            if (this.PiTasksStatus.ContainsKey(sample))
            {
                Console.WriteLine($"{sample} sample duplicate");
                return false;
            }
            this.PiTasksStatus[sample] = null;
            return true;
        }

        private void CompleteTask(PiTask piTask)
        {
            this.PiTasksStatus[piTask.Sample] = piTask;
        }

        public void StartMission()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    startMissionSemaphoreSlim.Wait();

                    long PiTaskSample = 0;

                    if (this.PiTaskSampleQueue.TryDequeue(out long result)) { PiTaskSample = result; }
                    else { continue; }

                    Task.Run(async () =>
                    {
                        Stopwatch sw = new Stopwatch();

                        sw.Start();
                        double piResult = await PiCalculator.Calculate(PiTaskSample);
                        sw.Stop();
                        double swTotal = sw.ElapsedMilliseconds;

                        PiTask task = new PiTask(PiTaskSample, swTotal.ToString(), piResult);
                        piTasks.Add(task);

                        CompleteTask(task);
                    });
                }
            });
        }

        private void PiTaskDTOTranslate()
        {
            foreach (var task in this.piTasks)
            {
                PiTaskDTO piTaskDTO = new PiTaskDTO(task);
                this.piTaskDTOs.Add(piTaskDTO);
            }
        }

        public void FetchCompleteMission()
        {
            PiTaskDTOTranslate();
            this.tasksView.RenderTask(this.piTaskDTOs.ToList());
            this.piTasks = new ConcurrentBag<PiTask>();
            this.piTaskDTOs = new ConcurrentBag<PiTaskDTO>();
        }

        public void StopMission()
        {
        }
    }
}
