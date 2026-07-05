using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PICalculatorDotNet8.Models
{
    class PiTaskDTO : INotifyPropertyChanged
    {
        private long _sample;
        public long Sample
        {
            get { return _sample; }
            set
            {
                _sample = value;
                OnPropertyChanged(nameof(Sample));
            }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        private ICommand _stopTaskCommand;
        public ICommand StopTaskCommand
        {
            get { return _stopTaskCommand; }
            set
            {
                _stopTaskCommand = value;
                OnPropertyChanged(nameof(StopTaskCommand));
            }
        }

        public PiTaskDTO(PiTask piTask)
        {
            this.Sample = piTask.Sample;
            this.Time = piTask.Time;
            this.Value = piTask.Value;
        }

        public PiTaskDTO(long Sample, ICommand command)
        {
            this.Sample = Sample;
            this.StopTaskCommand = command;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
