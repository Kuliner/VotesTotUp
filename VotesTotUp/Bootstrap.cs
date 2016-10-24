using System;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using ViewManagement;
using VotesTotUp.Data.Helpers;
using VotesTotUp.Managers;
using VotesTotUp.ViewModel;
using VotesTotUp.Views;

namespace VotesTotUp
{
    public class Bootstrap
    {
        private static UnityContainer _ioc;

        #region Methods

        internal async static void InitAsync(ContentControl windowContent)
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));

            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            Database.SetInitializer<DbModelContainer>(new CreateDatabaseIfNotExists<DbModelContainer>());
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");

            InitIoCManager();
            InitViewManager(windowContent, null);
            InitCurrentSessionManager();
            InitServices();

            LogManager.Instance.Init(log4net.LogManager.GetLogger(typeof(LogManager)));
            LogManager.Instance.LogInfo("Application is starting.");


            var viewManager = _ioc.Resolve<ViewManager>();
            viewManager.Init(windowContent);



            var csm = _ioc.Resolve<CurrentSessionManager>();
            await csm.InitAsync();
            _ioc.RegisterInstance<CurrentSessionManager>(csm);

            var db = _ioc.Resolve<DatabaseManager>();
            _ioc.RegisterInstance<DatabaseManager>(db);

        }

        private static void InitServices()
        {
            _ioc.RegisterType<Encryption>(new ContainerControlledLifetimeManager());
            _ioc.RegisterType<DbModelContainer>(new ContainerControlledLifetimeManager());
            _ioc.RegisterType<DatabaseManager>(new ContainerControlledLifetimeManager());
        }

        private static void InitCurrentSessionManager()
        {
            _ioc.RegisterType<CurrentSessionManager>(new ContainerControlledLifetimeManager());
        }

        private static void RegisterViews(ViewManager viewManager)
        {
            viewManager.RegisterViewModel<LoginViewModel, LoginView>();
            viewManager.RegisterViewModel<MainWindowViewModel, MainWindow>();
            viewManager.RegisterViewModel<StatisticsViewModel, StatisticsView>();
            viewManager.RegisterViewModel<VoterViewModel, VoterView>();
        }

        private static void InitIoCManager()
        {
            _ioc = new UnityContainer();
            _ioc.RegisterInstance(_ioc);

            var _iocManager = new IoCManager(_ioc);
            _ioc.RegisterInstance<IoCManager>(_iocManager);
        }

        private static void InitViewManager(ContentControl windowContent, ContentControl popUpContent)
        {
            _ioc.RegisterType<ViewManager>(new ContainerControlledLifetimeManager());

            var viewManager = _ioc.Resolve<ViewManager>();
            viewManager.Init(windowContent, popUpContent);
            _ioc.RegisterInstance<ViewManager>(viewManager);

            RegisterViews(viewManager);

        }

        #endregion Methods
    }
}