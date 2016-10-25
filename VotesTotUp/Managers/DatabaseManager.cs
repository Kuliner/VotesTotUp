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

        public DatabaseManager(DbModelContainer dbContext, Encryption encryption, PartyService partyService, VoterService voterService, CandidateService candidateService, StatisticService statisticService)
        {
            _dbContext = dbContext;

            Party = partyService;
            Voter = voterService;
            Candidate = candidateService;
            Statistic = statisticService;
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