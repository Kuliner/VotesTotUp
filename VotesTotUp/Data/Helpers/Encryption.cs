using Caelan.Frameworks.PasswordEncryption.Classes;
using Caelan.Frameworks.PasswordHashing.Classes;

namespace VotesTotUp.Data.Helpers
{
    public class Encryption
    {
        #region Fields

        private const string defaultPassword = "def4ult";
        private const string salt = "skąd Litwini wracali? Z nocnej wracali wycieczki";
        private const string secret = "secret";

        #endregion Fields

        #region Methods

        public string Decrypt(string password)
        {
            var encryptor = new PasswordEncryptor(defaultPassword, secret, salt);
            return encryptor.DecryptPassword(password);
        }

        public string Encrypt(string password)
        {
            var encryptor = new PasswordEncryptor(defaultPassword, secret, salt);
            return encryptor.EncryptPassword(password);
        }

        public string Hash(string password)
        {
            var encryptor = new PasswordHasher(salt, defaultPassword);
            return encryptor.HashPassword(password);
        }

        #endregion Methods
    }
}