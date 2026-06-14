using PICalculatorDotNet8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICalculatorDotNet8.Contract
{
    class TaskContract
    {
        internal interface ITaskPresenter
        {
            void StartMission();
            void AddTask(long sample);
            void FetchCompleteMission();
            void StopMission();
        }
        internal interface ITaskView
        {
            void OnAddedRenderTask(PiTaskDTO task);
            void RenderTask(List<PiTaskDTO> tasks);
        }
    }
}
