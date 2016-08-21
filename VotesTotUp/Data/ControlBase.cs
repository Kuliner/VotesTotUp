using GalaSoft.MvvmLight;

namespace VotesTotUp.Data
{
    public class ControlBase : ViewModelBase
    {
        #region Fields

        private long _invalidVotes;
        private string _name;
        private long _votes;

        #endregion Fields

        #region Properties

        public long InvalidVotes
        {
            get
            {
                return _invalidVotes;
            }

            set
            {
                _invalidVotes = value;
            }
        }
        public virtual string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public virtual long Votes
        {
            get
            {
                return _votes;
            }

            set
            {
                _votes = value;
            }
        }

        #endregion Properties
    }
}