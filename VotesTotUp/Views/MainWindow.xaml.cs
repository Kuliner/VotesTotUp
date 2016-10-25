using System.Windows;
using GalaSoft.MvvmLight;

namespace VotesTotUp.Views
{
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void Init()
        {
            BusyIndicator.IsBusy = true;
            Bootstrap.InitAsync(this.WindowContent);
            BusyIndicator.IsBusy = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => Init();

        #endregion Methods
    }
}