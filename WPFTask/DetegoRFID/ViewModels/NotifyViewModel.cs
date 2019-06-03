using DetegoRFID.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DetegoRFID.ViewModels
{
    public interface INotifyViewModel
    {
        void OnProgress(bool busy, string status);
        void OnSuccessMessage(string text);
        void OnFailMessage(string text);
        void OnFailMessage(Exception exception);
        void OnActiveMessage(string text, ActiveMessageType type);
        bool ExistsUnhanldeFail();
    }
    public class NotifyViewModel:NotifyPropertyChanged, INotifyViewModel
    {
        public ICommand MessageCloseCommand { get; set; }
        private bool isEnabled;
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                RaisePropertyChanged();
            }
        }
        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                RaisePropertyChanged();
            }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                RaisePropertyChanged();
            }
        }
        private ActiveMessage message;
        public ActiveMessage Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                RaisePropertyChanged();
            }
        }

        public NotifyViewModel parentModel;
        public NotifyViewModel ParentModel
        {
            get
            {
                return parentModel;
            }
            set
            {
                parentModel = value;
                RaisePropertyChanged();
            }
        }

        public NotifyViewModel() : this(null) { }

        public NotifyViewModel(NotifyViewModel parentModel)
        {
            this.ParentModel = parentModel;
            this.MessageCloseCommand = new Command(CloseMessage);
            this.IsEnabled = true;
        }

        public void OnProgress(bool isBusy, string status)
        {
            if (Message == null)
            {
                IsEnabled = !isBusy;
            }
            IsBusy = isBusy;
            Status = status;
        }

        public async void OnSuccessMessage(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                await Task.Delay(200);
                OnActiveMessage(text, ActiveMessageType.SUCCESS);
                await Task.Delay(text.Length * 200);
                CloseMessage();
            }
        }

        public void OnFailMessage(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                OnActiveMessage(text, ActiveMessageType.FAIL);
            }
        }

        public void OnActiveMessage(string text, ActiveMessageType type)
        {
            IsEnabled = false;
            Message = new ActiveMessage()
            {
                Text = text,
                Type = type
            };
        }

        public bool ExistsUnhanldeFail()
        {
            return Message != null && Message.Type == ActiveMessageType.FAIL;
        }

        private void CloseMessage()
        {
            Message = null;
            IsEnabled = true;
        }

        public void OnFailMessage(Exception exception)
        {
            if (exception != null)
            {
                IsEnabled = false;
                Message = new ActiveMessage()
                {
                    Text = exception.Message,
                    Type = ActiveMessageType.FAIL,
                    Exception = exception

                };
            }
        }
    }
}
