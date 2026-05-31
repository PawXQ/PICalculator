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
        public string _sampleText = "0";
        public string SampleText
        {
            get => _sampleText;
            set => _sampleText = value;
        }

        public MainViewModel()
        {
            taskPresenter = new TaskPresenter(this);

            //this.AddTaskCommand = new RelayCommand(AddTask, AddTaskCanExcute);
            this.AddTaskCommand = new RelayCommand(AddTask, () => true);
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
            this.taskPresenter.AddTask(long.Parse(this.SampleText));
        }
        public bool AddTaskCanExcute()
        {
            return string.IsNullOrEmpty(SampleText);
        }
    }
}
