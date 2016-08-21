using System.Linq;
using VotesTotUp.Managers;

namespace VotesTotUp.Data.Database.Statistic
{
    public class StatisticService
    {
        #region Fields

        private DbModelContainer _dbContext;

        #endregion Fields

        #region Constructors

        public StatisticService(DbModelContainer db)
        {
            _dbContext = db;
        }

        #endregion Constructors

        #region Methods

        public int Get()
        {
            try
            {
                return _dbContext.Statistics.First().BlockedAttempts;
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return -1;
            }
        }

        public void Update(int val)
        {
            try
            {
                if (_dbContext.Statistics.Count() == 0)
                {
                    _dbContext.Statistics.Add(new Statistics() { BlockedAttempts = 0 });
                    _dbContext.SaveChanges();
                }
                _dbContext.Statistics.First().BlockedAttempts = val;
                _dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
            }
        }

        #endregion Methods
    }
}