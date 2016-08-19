using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotesTotUp.Managers
{
    public class LogManager
    {
        #region Ctors (singleton)

        private static readonly log4net.ILog log =
               log4net.LogManager.GetLogger(typeof(LogManager));

        private static LogManager _instance;
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


        #endregion

        #region Public methods

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

        #endregion
    }
}
