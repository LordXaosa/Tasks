using DetegoRFID.Helpers;
using DetegoRFID.Services;
using DetegoRFID.ViewModels;
using DetegoRFID.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DetegoRFID
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IServiceManager
    {
        public static bool IsCriticalState = false;
        private bool _fatalExceptionSent = false;

        public IWindowService WindowService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MainExceptionHandler);
            DispatcherUnhandledException += MainExceptionHandler;

            base.OnStartup(e);
        }

        private void MainExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            HandleFatalException(e.Exception);
        }
        private void MainExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            HandleFatalException((Exception)e.ExceptionObject);
        }

        private void HandleFatalException(Exception exception)//smooth handling of all unexpected exceptions
        {
            if (!_fatalExceptionSent)
            {
                _fatalExceptionSent = true;//if few exceptions occured, we will show only one, to avoid recursion of exceptions if they occured at this part of code
                WindowService.ShowDialog(typeof(ExceptionWindow), exception);
                IsCriticalState = true;//for other threads, that could be in progress
                Shutdown();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Init();
        }

        private void Init()
        {
            WindowService = new WindowService();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            WindowService.Show(typeof(MainWindow), new MainViewModel());
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                RfidReader.Instance.Deactivate();
                RfidReader.Instance.Dispose();
            }
            catch
            {
                //could be logged if necessary
            }
        }
    }
}
