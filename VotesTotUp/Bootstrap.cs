using System;
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

        internal async static void InitAsync(ContentControl windowContent)
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));

            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            Database.SetInitializer<DbModelContainer>(new CreateDatabaseIfNotExists<DbModelContainer>());
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");

            LogManager.Instance.LogInfo("Application is starting.");
            ViewManager.Instance.Init(windowContent);

            await CurrentSessionManager.Instance.InitAsync();
        }

        #endregion Methods
    }
}