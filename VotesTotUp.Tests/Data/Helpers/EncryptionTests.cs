using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotesTotUp.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotesTotUp.Data.Helpers.Tests
{
    [TestClass()]
    public class EncryptionTests
    {
        [TestMethod()]
        public void DecryptTest()
        {
            var encryption = new Encryption();

            var password = "LOLOLOLO";
            var encryptedPassword = encryption.Encrypt(password);

            var decryptedPassword = encryption.Decrypt(encryptedPassword);

            Assert.AreEqual(password, decryptedPassword);
        }

        [TestMethod()]
        public void EncryptTest()
        {
            var encryption = new Encryption();

            var password = "LOLOLOLO";

            var encryptedPassword = encryption.Encrypt(password);

            Assert.AreNotEqual(password, encryptedPassword);
        }

        [TestMethod()]
        public void HashTest()
        {
            var encryption = new Encryption();

            var password = "LOLOLOLO";
            var hash = encryption.Hash(password);

            Assert.AreNotEqual(password, hash);

        }
    }
}