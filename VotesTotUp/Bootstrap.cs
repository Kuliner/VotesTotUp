using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VotesTotUp.Managers;

namespace VotesTotUp
{
    public class Bootstrap
    {
        internal static void Init(ContentControl windowContent)
        {
            LogManager.Instance.LogInfo("Application is starting.");
            ViewManager.Instance.Init(windowContent);

        }
    }
}
