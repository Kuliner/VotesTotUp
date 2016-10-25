using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotesTotUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotesTotUp.Managers;
using ViewManagement;
using Microsoft.Practices.Unity;

namespace VotesTotUp.ViewModel.Tests
{
    [TestClass()]
    public class LoginViewModelTests
    {
        private CurrentSessionManager currentSessionManager;
        private DatabaseManager databaseManager;

        public LoginViewModelTests()
        {
            databaseManager = new DatabaseManager(new DbModelContainer(), new Data.Helpers.Encryption());
            currentSessionManager = new CurrentSessionManager(new ViewManager(new IoCManager(new UnityContainer())), databaseManager);
        }


        [TestMethod()]
        public void ValidateNameTest()
        {
            var loginViewModel = new LoginViewModel(currentSessionManager, databaseManager);

            var validNameTestResult = false;
            var invalidNameTestResult = false;

            try
            {
                validNameTestResult = loginViewModel.ValidateName("Krzysiek", "Mucha");
            }
            catch (Exception)
            {
                validNameTestResult = false;
            }

            try
            {
                invalidNameTestResult = loginViewModel.ValidateName("99999", "];[.");
            }
            catch (Exception)
            {
                invalidNameTestResult = false;
            }

            Assert.IsFalse(invalidNameTestResult);
            Assert.IsTrue(validNameTestResult);
        }

        [TestMethod()]
        public void ValidatePeselTest()
        {
            var loginViewModel = new LoginViewModel(currentSessionManager, databaseManager);

            var validPeselTestResult = false;
            var invalidPeselTestResult = false;

            try
            {
                validPeselTestResult = loginViewModel.ValidatePesel("93081405174");
            }
            catch (Exception)
            {
                validPeselTestResult = false;
            }

            try
            {
                invalidPeselTestResult = loginViewModel.ValidatePesel("99999");
            }
            catch (Exception)
            {
                invalidPeselTestResult = false;
            }

            Assert.IsFalse(invalidPeselTestResult);
            Assert.IsTrue(validPeselTestResult);
        }
    }
}