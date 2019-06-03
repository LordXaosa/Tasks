using DetegoRFID.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DetegoRFID.Helpers
{
    public class ActiveMessage : NotifyPropertyChanged
    {

        public ActiveMessage()
        {
            OpenException = new Command(() => { (new ExceptionWindow() { DataContext = Exception }).Show(); });
        }

        public string Text { get; set; }
        public Exception Exception { get; set; }
        public ActiveMessageType Type { get; set; }

        private ICommand _openException;
        public ICommand OpenException
        {
            get => _openException;
            set
            {
                _openException = value;
                RaisePropertyChanged();
            }
        }

    }

    public enum ActiveMessageType : int
    {
        NONE = 0, INFO = 1, SUCCESS = 2, FAIL = 3
    }
}
