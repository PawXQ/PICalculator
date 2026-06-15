using PICalculatorDotNet8.Models;
using PICalculatorDotNet8.Presenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PICalculatorDotNet8.Contract.TaskContract;
using System.Windows.Input;
using System.Windows;

namespace PICalculatorDotNet8
{
    class MainViewModel : ITaskView
    {
        ITaskPresenter taskPresenter;
        public ICommand AddTaskCommand { get; set; }
        public ObservableCollection<PiTaskDTO> Tasks { get; set; } = new ObservableCollection<PiTaskDTO>();
        public Dictionary<long, PiTaskDTO> piTasksDict = new Dictionary<long, PiTaskDTO>();
        public string _sampleText = "0";
        public string SampleText
        {
            get => _sampleText;
            set => _sampleText = value;
        }

        public System.Threading.Timer FetchCompletedMissionTimer;

        public MainViewModel()
        {
            taskPresenter = new TaskPresenter(this);

            this.AddTaskCommand = new RelayCommand(AddTask, AddTaskCanExcute);

            this.FetchCompletedMissionTimer = new System.Threading.Timer(FetchCompletedMission, null, 0, 3000);
        }

        public void RenderTask(List<PiTaskDTO> tasks)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var task in tasks)
                {
                    piTasksDict[task.Sample].Sample = task.Sample;
                    piTasksDict[task.Sample].Time = task.Time;
                    piTasksDict[task.Sample].Value = task.Value;
                }
            });
        }

        public void OnAddedRenderTask(PiTaskDTO task)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Tasks.Add(task);
                piTasksDict[task.Sample] = task;
            });
        }

        public void AddTask()
        {
            this.taskPresenter.AddTask(long.Parse(this.SampleText));
        }

        public bool AddTaskCanExcute()
        {
            foreach (var task in this.Tasks)
            {
                if (task.Sample.ToString() == this.SampleText)
                {
                    MessageBox.Show($"{this.SampleText} sample duplicate");
                    return true;
                }
            }
            return true;
        }

        public void FetchCompletedMission(object state)
        {
            this.taskPresenter.FetchCompleteMission();
        }
    }
}
