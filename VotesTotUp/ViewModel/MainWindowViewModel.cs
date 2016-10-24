using GalaSoft.MvvmLight;

namespace VotesTotUp.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructors

        public MainWindowViewModel()
        {
            MessengerInstance.Register<bool>(this, payload => IsBusy = payload);
        }

        #endregion Constructors

        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }
    }
}