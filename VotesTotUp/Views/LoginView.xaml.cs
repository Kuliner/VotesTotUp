using System.Windows.Controls;

namespace VotesTotUp.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : ContentControl
    {
        #region Constructors

        public LoginView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        private void ContentControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                login.Command.Execute(this);
            }
        }
    }
}