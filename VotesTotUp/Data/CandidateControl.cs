using GalaSoft.MvvmLight;

namespace VotesTotUp.Data
{
    public class CandidateControl : ControlBase
    {
        #region Fields

        private string _party;
        private bool _vote;
        #endregion Fields

        #region Properties

 
        public string Party
        {
            get
            {
                return _party;
            }

            set
            {
                _party = value;
                RaisePropertyChanged();
            }
        }
        public bool Vote
        {
            get
            {
                return _vote;
            }

            set
            {
                _vote = value;
                RaisePropertyChanged();
            }
        }
     
        #endregion Properties
    }
}