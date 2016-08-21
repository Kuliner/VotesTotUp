namespace VotesTotUp.Data
{
    public class Enum
    {
        #region Enums

        public enum ExportType
        {
            Pdf, Csv
        }

        public enum Result
        {
            DoesntExist,
            PeselInDb,
            Success,
            Error
        }

        #endregion Enums
    }
}