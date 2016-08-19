using System.Data.Entity;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using VotesTotUp.Managers;

namespace VotesTotUp
{
    public class Bootstrap
    {
        #region Methods

        internal static void Init(ContentControl windowContent)
        {
            Database.SetInitializer<DbModelContainer>(new CreateDatabaseIfNotExists<DbModelContainer>());
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");

            LogManager.Instance.LogInfo("Application is starting.");
            ViewManager.Instance.Init(windowContent);

            CurrentSessionManager.Instance.Init();
        }

        #endregion Methods
    }
}
