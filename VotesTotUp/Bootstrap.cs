using System;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using ViewManager;
using VotesTotUp.Managers;
using VotesTotUp.ViewModel;
using VotesTotUp.Views;

namespace VotesTotUp
{
    public class Bootstrap
    {
        #region Methods

        internal async static void InitAsync(ContentControl windowContent)
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));

            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            Database.SetInitializer<DbModelContainer>(new CreateDatabaseIfNotExists<DbModelContainer>());
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");

            LogManager.Instance.Init(log4net.LogManager.GetLogger(typeof(LogManager)));
            LogManager.Instance.LogInfo("Application is starting.");
            ViewManager.ViewManager.Instance.Init(windowContent);

            RegisterViews();

            await CurrentSessionManager.Instance.InitAsync();
        }

        private static void RegisterViews()
        {
            ViewManager.ViewManager.Instance.RegisterViewModel<LoginViewModel, LoginView>();
            ViewManager.ViewManager.Instance.RegisterViewModel<MainWindowViewModel, MainWindow>();
            ViewManager.ViewManager.Instance.RegisterViewModel<StatisticsViewModel, StatisticsView>();
            ViewManager.ViewManager.Instance.RegisterViewModel<VoterViewModel, VoterView>();
        }

        #endregion Methods
    }
}