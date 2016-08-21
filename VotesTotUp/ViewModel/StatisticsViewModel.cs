using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VotesTotUp.Data;
using VotesTotUp.Managers;

namespace VotesTotUp.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        #region Fields

        private List<CandidateControl> _candidates = new List<CandidateControl>();
        private Visibility _chartDisplay = Visibility.Collapsed;
        private List<ControlBase> _data = new List<ControlBase>();
        private string _dataName;
        private DisplayMode _display;
        private DataToDisplay _displayData;
        private long _invalidVotes;
        private RelayCommand _logoutCommand;
        private Visibility _numberDisplay = Visibility.Visible;
        private List<PartyControl> _parties = new List<PartyControl>();
        private System.Timers.Timer _refreshDataTimer = new System.Timers.Timer();
        private RelayCommand _toggleDataCommand;
        private RelayCommand _toggleStatisticsDisplayCommand;
        private int _votes;

        #endregion Fields

        #region Constructors

        public StatisticsViewModel()
        {
            GetDataForDisplay();
            DisplayData = DataToDisplay.CandidateVotes;
            Display = DisplayMode.Numbers;
            _refreshDataTimer.AutoReset = false;
            _refreshDataTimer.Interval = 1000;
            _refreshDataTimer.Elapsed += _refreshDataTimer_Elapsed;
            _refreshDataTimer.Start();
        }

        #endregion Constructors

        #region Enums

        private enum DataToDisplay
        {
            PartyVotes,
            CandidateVotes
        }

        private enum DisplayMode
        {
            Charts,
            Numbers
        }

        #endregion Enums

        #region Properties

        public List<CandidateControl> Candidates
        {
            get
            {
                return _candidates;
            }
            set
            {
                _candidates = value;
                RaisePropertyChanged();
            }
        }

        public Visibility ChartDisplay
        {
            get
            {
                return _chartDisplay;
            }

            set
            {
                _chartDisplay = value;
                RaisePropertyChanged();
            }
        }

        public List<ControlBase> Data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
                RaisePropertyChanged();
            }
        }

        public string DataName
        {
            get
            {
                return _dataName;
            }

            set
            {
                _dataName = value;
                RaisePropertyChanged();
            }
        }

        public long InvalidVotes
        {
            get
            {
                return _invalidVotes;
            }

            set
            {
                _invalidVotes = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand LogoutCommand
        {
            get
            {
                if (_logoutCommand == null)
                    _logoutCommand = new RelayCommand(() =>
                    {
                        CurrentSessionManager.Instance.LogOut();
                    });
                return _logoutCommand;
            }
        }

        public Visibility NumberDisplay
        {
            get
            {
                return _numberDisplay;
            }

            set
            {
                _numberDisplay = value;
                RaisePropertyChanged();
            }
        }

        public List<PartyControl> Parties
        {
            get
            {
                return _parties;
            }
            set
            {
                _parties = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ToggleDataCommand
        {
            get
            {
                if (_toggleDataCommand == null)
                    _toggleDataCommand = new RelayCommand(() =>
                    {
                        if (DisplayData == DataToDisplay.PartyVotes)
                            DisplayData = DataToDisplay.CandidateVotes;
                        else
                            DisplayData = DataToDisplay.PartyVotes;
                    });
                return _toggleDataCommand;
            }
        }

        public RelayCommand ToggleStatisticsDisplayCommand
        {
            get
            {
                if (_toggleStatisticsDisplayCommand == null)
                    _toggleStatisticsDisplayCommand = new RelayCommand(() =>
                    {
                        if (Display == DisplayMode.Charts)
                            Display = DisplayMode.Numbers;
                        else
                            Display = DisplayMode.Charts;
                    });
                return _toggleStatisticsDisplayCommand;
            }
        }

        private DisplayMode Display
        {
            get
            {
                return _display;
            }

            set
            {
                _display = value;
                RefreshDataDisplay();
                RaisePropertyChanged();
            }
        }

        private DataToDisplay DisplayData
        {
            get
            {
                return _displayData;
            }

            set
            {
                _displayData = value;
                RefreshData();
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void _refreshDataTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetDataForDisplay();
            _refreshDataTimer.Start();
        }

        private bool CompareLists(List<ControlBase> first, List<ControlBase> second)
        {
            if (first.Count != second.Count)
                return false;
            else
            {
                first.OrderByDescending(x => x.Votes);
                second.OrderByDescending(x => x.Votes);

                for (int i = 0; i < first.Count; i++)
                {
                    if (first[i].Votes != second[i].Votes)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool DetectChanges(List<Voter> voters)
        {
            if (DisplayData == DataToDisplay.CandidateVotes && !CompareLists(Data, Candidates.Select(x => new ControlBase() { Name = x.Name, Votes = x.Votes }).ToList()))
            {
                return true;
            }

            if (DisplayData == DataToDisplay.PartyVotes && !CompareLists(Data, Parties.Select(x => new ControlBase() { Name = x.Name, Votes = x.Votes }).ToList()))
            {
                return true;
            }

            if (_votes != voters.Count)
            {
                _votes = voters.Count;
                return true;
            }

            return false;
        }

        private void GetDataForDisplay()
        {
            try
            {
                var candidates = DatabaseManager.Instance.Candidate.Get();
                var parties = DatabaseManager.Instance.Party.Get();
                var voters = DatabaseManager.Instance.Voter.Get();
                InvalidVotes = voters.Count(x => x.Voted && x.VoteValid == false);
                Candidates = candidates.Select(x => new CandidateControl { Name = x.Name, Party = x.Party.Name, Votes = x.Voters.Count(z => z.Voted && z.VoteValid), InvalidVotes = x.Voters.Count(y => y.Voted && y.VoteValid == false) }).ToList();
                Parties = new List<PartyControl>();
                foreach (var party in parties)
                {
                    var votes = 0;
                    var invalidVotes = 0;
                    foreach (var cand in party.Candidates)
                    {
                        var candVotes = 0;
                        var candInvalidVotes = 0;
                        foreach (var voter in cand.Voters)
                        {
                            if (voter.Voted && voter.VoteValid)
                                candVotes++;
                            else if (voter.Voted && !voter.VoteValid)
                                candInvalidVotes++;
                        }

                        votes += candVotes;
                        invalidVotes += candInvalidVotes;
                    }

                    Parties.Add(new PartyControl() { Name = party.Name, Votes = votes, InvalidVotes = invalidVotes });
                }

                if (DetectChanges(voters))
                    RefreshData();
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
            }
        }

        private void RefreshData()
        {
            try
            {
                if (DisplayData == DataToDisplay.CandidateVotes)
                {
                    DataName = "Candidates";
                    Data = Candidates.Select(x => new ControlBase() { Name = x.Name, Votes = x.Votes, InvalidVotes = x.InvalidVotes }).ToList();
                }
                else if (DisplayData == DataToDisplay.PartyVotes)
                {
                    DataName = "Parties";
                    Data = Parties.Select(x => new ControlBase() { Name = x.Name, Votes = x.Votes, InvalidVotes = x.InvalidVotes }).ToList();
                }

                RefreshDataDisplay();
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
            }
        }

        private void RefreshDataDisplay()
        {
            if (Display == DisplayMode.Numbers)
            {
                NumberDisplay = Visibility.Visible;
                ChartDisplay = Visibility.Collapsed;
            }
            else if (Display == DisplayMode.Charts)
            {
                NumberDisplay = Visibility.Collapsed;
                ChartDisplay = Visibility.Visible;
            }
        }

        #endregion Methods
    }
}