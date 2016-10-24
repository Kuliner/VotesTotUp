using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VotesTotUp.Data;
using VotesTotUp.Managers;
using ViewManagement;

namespace VotesTotUp.ViewModel
{
    public class VoterViewModel : ViewModelBase
    {
        private CurrentSessionManager _currentSessionManager;
        private DatabaseManager _databaseManager;
        #region Fields

        private RelayCommand _logoutCommand;
        private RelayCommand _statisticsCommand;
        private ViewManager _viewManager;
        private RelayCommand _voteCommad;
        private bool _voted;
        private Voter _voter;

        #endregion Fields

        #region Constructors

        public VoterViewModel(CurrentSessionManager currentSessionManager, DatabaseManager databaseManager, ViewManager viewManager)
        {
            _currentSessionManager = currentSessionManager;
            _databaseManager = databaseManager;
            _viewManager = viewManager;

            _voter = _currentSessionManager.CurrentlyLoggedVoter;
            Voted = _voter.Voted;

            var candidates = _databaseManager.Candidate.Get();
            Candidates = candidates.Select(x => new CandidateControl { Name = x.Name, Party = x.Party.Name, Vote = false }).ToList();
        }

        #endregion Constructors

        #region Properties

        public List<CandidateControl> Candidates { get; set; } = new List<CandidateControl>();
        public RelayCommand LogoutCommand
        {
            get
            {
                if (_logoutCommand == null)
                    _logoutCommand = new RelayCommand(() =>
                    {
                        if (!Voted)
                        {
                            if (Candidates.FirstOrDefault(x => x.Vote == true) != null)
                            {
                                MessageBoxResult result = MessageBox.Show("Are you sure you want to log out? There is some voting operations in progress and not confirmed", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);

                                if (result == MessageBoxResult.Yes)
                                {
                                    LogOut();
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                        LogOut();
                    });
                return _logoutCommand;
            }
        }
        public RelayCommand StatisticsCommand
        {
            get
            {
                if (_statisticsCommand == null)
                    _statisticsCommand = new RelayCommand(() =>
                    {
                        _viewManager.OpenView<StatisticsViewModel>();
                    });
                return _statisticsCommand;
            }
        }

        public RelayCommand VoteCommand
        {
            get
            {
                if (_voteCommad == null)
                    _voteCommad = new RelayCommand(() =>
                    {
                        var result = MessageBox.Show("Are you sure you want to vote?", "Vote", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                            Vote();
                    });
                return _voteCommad;
            }
        }

        public bool Voted
        {
            get
            {
                return _voted;
            }

            set
            {
                _voted = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void LogOut()
        {
            _currentSessionManager.LogOut();
        }

        private void Vote()
        {
            if (Candidates.Where(x => x.Vote).Count() == 1)
            {
                _voter.VoteValid = true;
                _voter.Voted = true;
                Voted = true;
            }
            else
            {
                _voter.VoteValid = false;
                _voter.Voted = true;
                Voted = true;
            }

            foreach (var candidate in Candidates.Where(X => X.Vote == true))
            {
                _voter.Candidates.Add(_databaseManager.Candidate.Get(candidate.Name));
            }

            _databaseManager.Voter.Update(_voter);
            _viewManager.OpenView<StatisticsViewModel>();
        }

        #endregion Methods
    }
}