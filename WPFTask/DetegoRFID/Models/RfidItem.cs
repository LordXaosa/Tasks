using DetegoRFID.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetegoRFID.Models
{
    public class RfidItem : NotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        private int _overallCount;

        public int OverallCount
        {
            get { return _overallCount; }
            set
            {
                _overallCount = value;
                RaisePropertyChanged();
            }
        }

        private int _timesSeenLastMinute;

        public int TimesSeenLastMinute
        {
            get { return _timesSeenLastMinute; }
            set
            {
                _timesSeenLastMinute = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _lastSeen;

        public DateTime LastSeen
        {
            get { return _lastSeen; }
            set
            {
                _lastSeen = value;
                RaisePropertyChanged();
            }
        }

        private bool _recentlyUpdated;

        public bool RecentlyUpdated
        {
            get { return _recentlyUpdated; }
            set
            {
                _recentlyUpdated = value;
                RaisePropertyChanged();
            }
        }

    }
}
