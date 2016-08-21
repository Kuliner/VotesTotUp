using System;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VotesTotUp.Managers;
using static VotesTotUp.Data.Enum;

namespace VotesTotUp.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        private RelayCommand _debugCommand;
        private string _firstName;
        private string _lastName;
        private RelayCommand _loginCommand;
        private string _pesel = null;

        #endregion Fields

        #region Properties

        public RelayCommand DebugCommand
        {
            get
            {
                if (_debugCommand == null)
                    _debugCommand = new RelayCommand(() =>
                    {
                        //FirstName = RandomNameGenerator.NameGenerator.GenerateFirstName(RandomNameGenerator.Gender.Male);
                        //LastName = RandomNameGenerator.NameGenerator.GenerateLastName();
                        //Pesel = RandomDigits(11);

                        FirstName = "Rafał";
                        LastName = "Dunaj";
                        Pesel = "92081504179";
                    });
                return _debugCommand;
            }
        }
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
        public RelayCommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(() =>
                    {
                        Login();
                    });
                }
                return _loginCommand;
            }
        }
        public string Pesel
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

        #endregion Properties

        #region Methods

        private void Login()
        {
            try
            {
                ValidateName(FirstName, LastName);
                ValidatePesel(Pesel);
                Result result;
                var userInDb = DatabaseManager.Instance.Voter.Get(FirstName, LastName, Pesel, out result);
                if (userInDb == null)
                {
                    if (result == Result.DoesntExist)
                    {
                        var voter = new Voter { Pesel = Pesel, FirstName = FirstName, LastName = LastName, Voted = false };
                        DatabaseManager.Instance.Voter.Add(voter);
                    }
                    else if (result == Result.PeselInDb)
                    {
                        MessageBox.Show("Voter with such Pesel number already exists in the database, check first name and last name spelling", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else if (result == Result.Error)
                    {
                        MessageBox.Show("There has been some error, please consider contacting support team", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                CurrentSessionManager.Instance.LoginVoter(DatabaseManager.Instance.Voter.Get(Pesel), Pesel);
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBoxResult result = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        private string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        private void ValidateName(string firstName, string lastName)
        {
            if (firstName.Any(x => char.IsDigit(x)))
                throw new Exception("Name must not contain any letters or digits!");
            if (lastName.Any(x => char.IsDigit(x)))
                throw new Exception("Name must not contain any letters or digits!");
        }

        private void ValidatePesel(string pesel)
        {
            pesel = pesel.Replace(" ", "");

            if (pesel.Any(x => char.IsNumber(x) == false))
                throw new Exception("Pesel must contain only numbers!");

            if (pesel.Length != 11)
                throw new Exception("Pesel must contain 11 digits!");
        }

        #endregion Methods
    }
}