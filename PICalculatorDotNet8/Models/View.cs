using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PICalculatorDotNet8.Models
{
    internal class View : INotifyPropertyChanged
    {
        private string _addText = "Add";
        public string AddText
        {
            get { return _addText; }
            set
            {
                _addText = value;
            }
        }

        private bool _isStart = true;
        public bool IsStart
        {
            get { return _isStart; }
            set
            {
                _isStart = value;
                OnPropertyChanged(nameof(IsStart));
                OnPropertyChanged(nameof(OperateStateText));
            }
        }

        public string OperateStateText => this.IsStart ? "Stop" : "Start";

        public ICommand ChangeOperateStatusCommand { get; set; }

        public View()
        {
            this.ChangeOperateStatusCommand = new RelayCommand(ChangeOperateStatus, () => true);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChangeOperateStatus()
        {
            this.IsStart = !this.IsStart;
        }
    }
}
