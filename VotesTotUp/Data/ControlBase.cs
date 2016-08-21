using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace VotesTotUp.Data
{
    public class ControlBase : ViewModelBase
    {
        private string _name;
        private long _votes;

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

        private long _invalidVotes;

    }
}
