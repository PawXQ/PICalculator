using PICalculator.Presenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static PICalculator.Contract.TaskContract;

namespace PICalculator
{
    internal class MainViewModel : ITaskView, INotifyPropertyChanged
    {
        ITaskPresenter taskPresenter;
        public ICommand AddTaskCommand { get; set; }
        public ObservableCollection<PiTask> Tasks { get; set; } = new ObservableCollection<PiTask>();
        public string _sampleText = "0";
        public string SampleText
        {
            get => _sampleText;
            set => _sampleText = value;
        }

        public MainViewModel()
        {
            taskPresenter = new TaskPresenter(this);

            this.AddTaskCommand = new RelayCommand(AddTask, AddTaskCanExcute);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RenderTask(PiTask piTask)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < Tasks.Count; i++)
                {
                    if (piTask.Sample == Tasks[i].Sample)
                    {
                        Tasks[i] = piTask;
                        return;
                    }
                }
                this.Tasks.Add(piTask);
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
                    MessageBox.Show("SampleText duplicate");
                    return false;
                }
            }
            return true;
        }
    }
}
