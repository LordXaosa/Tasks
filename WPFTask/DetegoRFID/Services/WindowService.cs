using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DetegoRFID.Services
{
    public interface IWindowService
    {
        void Show(Type type, object dataContext);
        bool ShowDialog(Type type, object dataContext);
        bool ShowDialog(Type type, object dataContext, bool showInTaskbar);
        bool ShowDialog(Type type, object dataContext, Window owner);
        bool ShowDialog(Type type, object dataContext, Window owner, bool showInTaskbar);
    }

    public class WindowService : IWindowService
    {
        public void Show(Type type, object dataContext)
        {
            var window = (Window)Activator.CreateInstance(type);
            window.DataContext = dataContext;
            window.Show();
        }

        private Window GetDefaultOwner()
        {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
        }

        public bool ShowDialog(Type type, object dataContext)
        {
            return ShowDialog(type, dataContext, GetDefaultOwner(), false);
        }

        public bool ShowDialog(Type type, object dataContext, bool showInTaskbar)
        {
            return ShowDialog(type, dataContext, GetDefaultOwner(), showInTaskbar);
        }

        public bool ShowDialog(Type type, object dataContext, Window owner)
        {
            return ShowDialog(type, dataContext, owner, false);
        }

        public bool ShowDialog(Type type, object dataContext, Window owner, bool showInTaskbar)
        {
            var window = (Window)Activator.CreateInstance(type);
            window.DataContext = dataContext;
            window.Owner = owner;
            window.ShowInTaskbar = showInTaskbar;
            return window.ShowDialog() == true;
        }
    }
}
