using VotesTotUp.Data.Database.Services;
using VotesTotUp.Data.Database.Statistic;

namespace VotesTotUp.Managers
{
    public class DatabaseManager
    {
        #region Fields

        private static DatabaseManager _instance;

        private DbModelContainer _dbContext;

        #endregion Fields

        #region Constructors

        private DatabaseManager()
        {
            _dbContext = new DbModelContainer();

            Party = new PartyService(_dbContext);
            Voter = new VoterService(_dbContext);
            Candidate = new CandidateService(_dbContext);
            Statistic = new StatisticService(_dbContext);
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
        public StatisticService Statistic { get; set; }
        public VoterService Voter { get; set; }

        #endregion Properties
    }
}