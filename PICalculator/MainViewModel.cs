using PICalculator.Presenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static PICalculator.Contract.TaskContract;

namespace PICalculator
{
    internal class MainViewModel : ITaskView, INotifyPropertyChanged
    {
        ITaskPresenter taskPresenter;
        public ICommand AddTaskCommand { get; set; }
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
        public long SampleText { get; set; }

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

        public void RenderTask(Task task)
        {
            this.Tasks.Add(task);
        }

        public void AddTask()
        {
            this.taskPresenter.AddTask(this.SampleText);
        }
        public bool AddTaskCanExcute()
        {
            return true;
        }
    }
}
