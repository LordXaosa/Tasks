using DetegoRFID.Helpers;
using DetegoRFID.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DetegoRFID.ViewModels
{
    public class MainViewModel : NotifyViewModel
    {
        MyDictionary<string, RfidItem> _data;
        public MyDictionary<string, RfidItem> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged();
            }
        }

        private List<KeyValuePair<string, DateTime>> minuteArray = new List<KeyValuePair<string, DateTime>>();//stores data about recently seen tags
        public ICommand ActivateCommand { get; set; }
        public ICommand DeactivateCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                RaisePropertyChanged();
            }
        }
        public MainViewModel()
        {
            Data = new MyDictionary<string, RfidItem>();
            RfidReader.Instance.TagSeen += TagSeenFire;
            ActivateCommand = new Command(Activate, x => !IsActive);
            DeactivateCommand = new Command(Deactivate, x => IsActive);
            ResetCommand = new Command(Reset);
        }

        private void TagSeenFire(object sender, Rfid.TagSeenEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => //dispatcher used just to invoke collection changes in UI thread
            {
                UncheckUpdate();
                minuteArray.Add(new KeyValuePair<string, DateTime>(e.Identifier, DateTime.Now));
                if (Data.ContainsKey(e.Identifier))
                {
                    Data[e.Identifier].LastSeen = DateTime.Now;
                    Data[e.Identifier].OverallCount = Data[e.Identifier].OverallCount + 1;
                    Data[e.Identifier].TimesSeenLastMinute = minuteArray.Count(p => p.Key == e.Identifier);
                    Data[e.Identifier].RecentlyUpdated = true;
                }
                else
                {
                    RfidItem item = new RfidItem()
                    {
                        LastSeen = DateTime.Now,
                        Name = e.Identifier,
                        OverallCount = 1,
                        TimesSeenLastMinute = 1,
                        RecentlyUpdated = true
                    };
                    Data.Add(e.Identifier, item);
                }
                
                minuteArray = minuteArray.Where(p => p.Value > DateTime.Now.AddMinutes(-1)).ToList();//clear too old entries
            }));
        }
        void UncheckUpdate()
        {
            foreach(var item in Data)
            {
                item.Value.RecentlyUpdated = false;
            }
        }
        public void Activate()
        {
            RfidReader.Instance.Activate();
            IsActive = true;
        }

        public void Deactivate()
        {
            RfidReader.Instance.Deactivate();
            IsActive = false;
        }

        public void Reset()
        {
            Data.Clear();
            minuteArray.Clear();
        }
    }
}
