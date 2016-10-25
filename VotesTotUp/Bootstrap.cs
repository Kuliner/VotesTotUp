using System;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using ViewManagement;
using VotesTotUp.Data.Database.Services;
using VotesTotUp.Data.Helpers;
using VotesTotUp.Managers;
using VotesTotUp.ViewModel;
using VotesTotUp.Views;
using Xceed.Wpf.Toolkit;

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
            InitLogger();
            InitViewManager(windowContent, null);
            InitCurrentSessionManager();
            InitServices();

            var viewManager = _ioc.Resolve<ViewManager>();
            viewManager.Init(windowContent);

            var csm = _ioc.Resolve<CurrentSessionManager>();
            await csm.InitAsync();
        }

        private static void InitLogger()
        {
            _ioc.RegisterType<LogManager>(new ContainerControlledLifetimeManager());

            var logger = _ioc.Resolve<LogManager>();
            logger.Init(log4net.LogManager.GetLogger(typeof(LogManager)));
            logger.LogInfo("Application is starting.");
        }

        private static void InitServices()
        {
            _ioc.RegisterType<Encryption>(new ContainerControlledLifetimeManager());
            _ioc.RegisterType<DbModelContainer>(new ContainerControlledLifetimeManager());
            _ioc.RegisterType<DatabaseManager>(new ContainerControlledLifetimeManager());
            _ioc.RegisterType<CandidateService>(new ContainerControlledLifetimeManager());
            _ioc.RegisterType<VoterService>(new ContainerControlledLifetimeManager());
            _ioc.RegisterType<PartyService>(new ContainerControlledLifetimeManager());
            _ioc.RegisterType<Data.Database.Statistic.StatisticService>(new ContainerControlledLifetimeManager());
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
            RegisterViews(viewManager);
        }

        #endregion Methods
    }
}