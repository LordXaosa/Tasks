using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DetegoRFID.Helpers
{
    public class MyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        Dictionary<TKey, TValue> dict;
        public MyDictionary()
        {
            dict = new Dictionary<TKey, TValue>();
        }
        public TValue this[TKey index]
        {
            get
            {
                return dict[index];
            }

            set
            {
                dict[index] = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(Binding.IndexerName));
                }
            }
        }

        public ICollection<TKey> Keys => dict.Keys;

        public ICollection<TValue> Values => dict.Values;

        public int Count => dict.Count;

        public bool IsReadOnly => false;

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(TKey key, TValue value)
        {
            dict.Add(key, value);
            OnPropertyChanged("Count");
            OnPropertyChanged(Binding.IndexerName);
            OnCollectionReset();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dict.Add(item.Key, item.Value);
            OnPropertyChanged("Count");
            OnPropertyChanged(Binding.IndexerName);
            OnCollectionReset();
        }

        public void Clear()
        {
            dict.Clear();
            OnPropertyChanged("Count");
            OnPropertyChanged(Binding.IndexerName);
            OnCollectionReset();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dict.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            var result = dict.Remove(key);
            OnPropertyChanged("Count");
            OnPropertyChanged(Binding.IndexerName);
            OnCollectionReset();
            return result;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var result = dict.Remove(item.Key);
            OnPropertyChanged("Count");
            OnPropertyChanged(Binding.IndexerName);
            OnCollectionReset();//not really good solution, it will cause redraw of all items in list after each update
            return result;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
        private void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
