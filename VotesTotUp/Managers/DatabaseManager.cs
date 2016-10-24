using VotesTotUp.Data.Database.Services;
using VotesTotUp.Data.Database.Statistic;
using VotesTotUp.Data.Helpers;

namespace VotesTotUp.Managers
{
    public class DatabaseManager
    {
        #region Fields

        private DbModelContainer _dbContext;

        #endregion Fields

        #region Constructors

        public DatabaseManager(DbModelContainer dbContext, Encryption encryption)
        {
            _dbContext = new DbModelContainer();

            Party = new PartyService(_dbContext);
            Voter = new VoterService(_dbContext, encryption);
            Candidate = new CandidateService(_dbContext);
            Statistic = new StatisticService(_dbContext);
        }

        #endregion Constructors

        #region Properties

        public CandidateService Candidate { get; set; }
        public PartyService Party { get; set; }
        public StatisticService Statistic { get; set; }
        public VoterService Voter { get; set; }

        #endregion Properties
    }
}