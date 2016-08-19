using VotesTotUp.Data.Services;

namespace VotesTotUp.Managers
{
    public class DatabaseManager
    {
        #region Fields

        private static DatabaseManager _instance;

        #endregion Fields

        #region Constructors

        private DatabaseManager()
        {
            Party = new PartyService();
            Voter = new VoterService();
            Candidate = new CandidateService();
        }

        #endregion Constructors

        #region Properties

        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseManager();
                }
                return _instance;
            }
        }

        public CandidateService Candidate { get; set; }
        public PartyService Party { get; set; }
        public VoterService Voter { get; set; }

        #endregion Properties
    }
}