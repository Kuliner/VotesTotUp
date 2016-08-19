using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using VotesTotUp.Data;
using VotesTotUp.ViewModel;
using VotesTotUp.Views;

namespace VotesTotUp.Managers
{
    public class ViewManager
    {
        #region Fields

        private static ViewManager _instance;
        private ContentControl _content;
        private List<ViewModelConnection> _views = new List<ViewModelConnection>();

        #endregion Fields

        #region Constructors

        private ViewManager()
        {
        }

        #endregion Constructors

        #region Properties

        public static ViewManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ViewManager();
                return _instance;
            }
        }

        #endregion Properties

        #region Methods

        public void Init(ContentControl content)
        {
            _content = content;

            RegisterViewModels();

            OpenView<LoginViewModel>();
        }

        public void OpenView<VM>() where VM : ViewModelBase
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                OpenViewBlocking(typeof(VM));
            }), (System.Windows.Threading.DispatcherPriority)10, null);
        }

        public void OpenViewBlocking(Type VM)
        {
            // Get item for passed viewModel type
            ViewModelConnection item = _views.FirstOrDefault(x => x.ViewModelType == VM);
            if (item == null)
                throw new Exception("View not registered.");

            ContentControl view = GetView(item);

            ViewModelBase viewModel = view.DataContext as ViewModelBase;
            if (viewModel == null)
                viewModel = GetViewModel(item);

            view.DataContext = viewModel;
            _content.Content = view;
        }

        public void RegisterViewModel<VM, V>()
            where VM : ViewModelBase
            where V : ContentControl
        {
            var item = new ViewModelConnection();
            item.Set<VM, V>();
            _views.Add(item);
        }

        protected ContentControl GetView(ViewModelConnection item) => Activator.CreateInstance(item.ViewType) as ContentControl;

        protected virtual ViewModelBase GetViewModel(ViewModelConnection item) => Activator.CreateInstance(item.ViewModelType) as ViewModelBase;

        private void RegisterViewModels()
        {
            RegisterViewModel<MainWindowViewModel, MainWindow>();
            RegisterViewModel<LoginViewModel, LoginView>();
            RegisterViewModel<CalculatorViewModel, CalculatorView>();
        }

        #endregion Methods
    }
}