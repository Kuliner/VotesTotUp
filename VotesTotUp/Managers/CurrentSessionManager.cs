using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VotesTotUp.Data.Helpers;
using VotesTotUp.ViewModel;

namespace VotesTotUp.Managers
{
    public class CurrentSessionManager
    {
        #region Fields

        private const string _blockedUrl = "http://webtask.future-processing.com:8069/blocked";
        private const string _candidatesUrl = "http://webtask.future-processing.com:8069/candidates";
        private static CurrentSessionManager _instance;
        private List<string> _blockedPesels = new List<string>();
        #endregion Fields

        #region Constructors

        private CurrentSessionManager()
        {
            Encryptor = new Encryption();
        }

        #endregion Constructors

        #region Properties

        public static CurrentSessionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CurrentSessionManager();
                return _instance;
            }
        }

        public Voter CurrentlyLoggedVoter { get; set; }
        public Encryption Encryptor { get; set; }

        #endregion Properties

        #region Methods

        public async Task InitAsync()
        {
            try
            {
                JToken candidates = await GetJsonCandidatesAsync();
                JToken blocked = await GetJsonBlockedAsync();

                foreach (var person in blocked)
                {
                    _blockedPesels.Add(Encryptor.Hash(person.Value<string>("pesel")));
                }

                PopulateDatabase(candidates);

                ViewManager.Instance.OpenView<LoginViewModel>();
            }
            catch (WebException wex)
            {
                var pageContent = new StreamReader(wex.Response.GetResponseStream())
                                      .ReadToEnd();
            }
        }

        public void LoginVoter(Voter voter, string pesel)
        {
            if (!CheckVotersPrivilege(voter, pesel))
                return;

            CurrentlyLoggedVoter = voter;
            if (!CurrentlyLoggedVoter.Voted)
                ViewManager.Instance.OpenView<VoterViewModel>();
            else
                ViewManager.Instance.OpenView<StatisticsViewModel>();
        }

        private bool CheckVotersPrivilege(Voter voter, string strpesel)
        {
            //Check your privilege!
            if (_blockedPesels.Contains(voter.Pesel))
            {
                MessageBox.Show("You're not allowed to vote", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var pesel = new int[11];
            for (int i = 0; i < 11; i++)
            {
                pesel[i] = int.Parse(strpesel[i].ToString());
            }

            var birthday = GetYear(pesel);
            var yearsGap = (DateTime.Now - birthday).TotalDays / 365.25;
            if (yearsGap < 18)
            {
                MessageBox.Show("You're not allowed to vote", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private DateTime GetYear(int[] pesel)
        {
            var year = 1900 + pesel[0] * 10 + pesel[1];
            if (pesel[2] >= 2 && pesel[2] < 8)
                year += pesel[2] / 2 * 100;
            if (pesel[2] >= 8)
                year -= 100;

            var month = (pesel[2] % 2) * 10 + pesel[3];
            var day = pesel[4] * 10 + pesel[5];

            return new DateTime(year, month, day);
        }

        public void LogOut()
        {
            CurrentlyLoggedVoter = null;
            ViewManager.Instance.OpenView<LoginViewModel>();
        }

        private async Task<JToken> GetJsonBlockedAsync()
        {
            string pageContent = await GetJsonContentAsync(_blockedUrl);

            var results = JsonConvert.DeserializeObject<JObject>(pageContent);
            return results.SelectToken("disallowed").SelectToken("person");
        }

        private async Task<JToken> GetJsonCandidatesAsync()
        {
            string pageContent = await GetJsonContentAsync(_candidatesUrl);

            var results = JsonConvert.DeserializeObject<JObject>(pageContent);
            return results.SelectToken("candidates").SelectToken("candidate");
        }

        private async Task<string> GetJsonContentAsync(string url)
        {
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.ContentType = "application / json";
            req.Accept = "application / json";
            WebResponse wr = await req.GetResponseAsync();

            return new StreamReader(wr.GetResponseStream())
                                                 .ReadToEnd();
        }

        private void PopulateDatabase(JToken candidates)
        {
            Task.Factory.StartNew(() =>
            {
                var candidatesList = DatabaseManager.Instance.Candidate.Get();
                if (candidatesList == null || candidatesList.Count != candidates.Count())
                {
                    foreach (var candidate in candidates)
                    {
                        var name = candidate.Value<string>("name");
                        var party = candidate.Value<string>("party");

                        if (DatabaseManager.Instance.Party.Get(party) == null)
                        {
                            var partyEntity = new Party { Name = party };
                            DatabaseManager.Instance.Party.Add(partyEntity);
                        }

                        if (DatabaseManager.Instance.Candidate.Get(name) == null)
                        {
                            var cand = new Candidate { Name = name };
                            var partyEntity = DatabaseManager.Instance.Party.Get(party);

                            DatabaseManager.Instance.Candidate.Add(new Candidate { Name = name, Party = partyEntity });
                        }
                    }
                }
            });
        }

        #endregion Methods
    }
}