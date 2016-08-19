namespace VotesTotUp.Managers
{
    public class LogManager
    {
        #region Fields

        private static readonly log4net.ILog log =
               log4net.LogManager.GetLogger(typeof(LogManager));

        private static LogManager _instance;

        #endregion Fields

        #region Properties

        public static LogManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LogManager();
                }
                return _instance;
            }
        }

        #endregion Properties

        #region Methods

        public void LogError(string message)
        {
            log.Error(message);
        }

        public void LogInfo(string message)
        {
            log.Info(message);
        }

        public void LogWarning(string message)
        {
            log.Warn(message);
        }

        #endregion Methods
    }
}