using System;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VotesTotUp.Managers;

namespace VotesTotUp.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private int? _pesel = null;
        public int? Pesel
        {
            get
            {
                return _pesel;
            }
            set
            {
                _pesel = value;
                RaisePropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                RaisePropertyChanged();
            }
        }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(() =>
                    {
                        try
                        {
                            ValidateName(FirstName, LastName);
                            ValidatePesel(Pesel);
                        }
                        catch (Exception ex)
                        {
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                MessageBoxResult result = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            });
                        }
                    });
                }
                return _loginCommand;
            }
        }

        private void ValidatePesel(int? pesel)
        {
            if (pesel.ToString().Length != 11)
                throw new Exception("Pesel must contain 11 digits!");
        }

        private void ValidateName(string firstName, string lastName)
        {
            if (firstName.Any(x => char.IsDigit(x)))
                throw new Exception("Name must not contain any letters or digits!");
            if (lastName.Any(x => char.IsDigit(x)))
                throw new Exception("Name must not contain any letters or digits!");
        }
    }
}