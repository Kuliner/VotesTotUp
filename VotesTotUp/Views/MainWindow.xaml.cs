using System.Windows;

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
            Bootstrap.InitAsync(this.WindowContent);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => Init();

        #endregion Methods
    }
}