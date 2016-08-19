using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VotesTotUp.ViewModel;

namespace VotesTotUp.Managers
{
    public class CurrentSessionManager
    {
        #region Fields

        private static CurrentSessionManager _instance;

        private string _blockedUrl = "http://webtask.future-processing.com:8069/blocked";

        private string _candidatesUrl = "http://webtask.future-processing.com:8069/candidates";

        #endregion Fields

        #region Constructors

        private CurrentSessionManager()
        {
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

        #endregion Properties

        #region Methods

        public void Init()
        {
            try
            {
                JToken candidates = GetJsonCandidates();
                JToken blocked = GetJsonBlocked();

                PopulateDatabase(candidates);

                ViewManager.Instance.OpenView<LoginViewModel>();
            }
            catch (WebException wex)
            {
                var pageContent = new StreamReader(wex.Response.GetResponseStream())
                                      .ReadToEnd();
            }
        }

        private JToken GetJsonBlocked()
        {
            string pageContent = GetJsonContent(_blockedUrl);

            var results = JsonConvert.DeserializeObject<JObject>(pageContent);
            return results.SelectToken("disallowed").SelectToken("person");
        }

        private JToken GetJsonCandidates()
        {
            string pageContent = GetJsonContent(_candidatesUrl);

            var results = JsonConvert.DeserializeObject<JObject>(pageContent);
            return results.SelectToken("candidates").SelectToken("candidate");
        }

        private string GetJsonContent(string url)
        {
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.ContentType = "application / json";
            req.Accept = "application / json";
            WebResponse wr = req.GetResponse();

            return new StreamReader(wr.GetResponseStream())
                                                 .ReadToEnd();
        }

        private void PopulateDatabase(JToken candidates)
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
        }

        #endregion Methods
    }
}