﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using DoddleReport;
using DoddleReport.Writers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VotesTotUp.Data;
using VotesTotUp.Data.Helpers;
using VotesTotUp.ViewModel;
using static VotesTotUp.Data.Enum;
using ViewManager;

namespace VotesTotUp.Managers
{
    public class CurrentSessionManager
    {
        #region Fields

        private const string _blockedUrl = "http://webtask.future-processing.com:8069/blocked";
        private const string _candidatesUrl = "http://webtask.future-processing.com:8069/candidates";
        private static CurrentSessionManager _instance;
        private int _blockedAttempt;
        private List<string> _blockedPesels = new List<string>();
        private System.Timers.Timer _blockRefresher = new System.Timers.Timer();

        #endregion Fields

        #region Constructors

        private CurrentSessionManager()
        {
            Encryptor = new Encryption();
            _blockRefresher.AutoReset = false;
            _blockRefresher.Interval = 1000;
            _blockRefresher.Elapsed += _blockRefresher_ElapsedAsync;
            _blockRefresher.Start();
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

        public void Export(List<CandidateControl> cands, List<PartyControl> parts, string path, ExportType type)
        {
            try
            {
                using (var fileStream = File.Create($"{path}\\Candidates.{type.ToString()}"))
                {
                    Report candReport = PrepCandReport(cands);

                    if (type == ExportType.Pdf)
                    {
                        var writer = new DoddleReport.iTextSharp.PdfReportWriter();
                        writer.WriteReport(candReport, fileStream);
                    }
                    else
                    {
                        var writer = new DoddleReport.Writers.DelimitedTextReportWriter();
                        DelimitedTextReportWriter.DefaultDelimiter = ",";
                        writer.WriteReport(candReport, fileStream);
                    }
                }
                using (var fileStream = File.Create($"{path}\\Parties.{type.ToString()}"))
                {
                    Report partiesReport = PrepPartyReport(parts);

                    if (type == ExportType.Pdf)
                    {
                        var writer = new DoddleReport.iTextSharp.PdfReportWriter();
                        writer.WriteReport(partiesReport, fileStream);
                    }
                    else
                    {
                        var writer = new DoddleReport.Writers.DelimitedTextReportWriter();
                        DelimitedTextReportWriter.DefaultDelimiter = ",";
                        writer.WriteReport(partiesReport, fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task InitAsync()
        {
            try
            {
                await GetBlockedVotersAsync();
                await PopulateDatabaseAsync();

                ViewManager.ViewManager.Instance.OpenView<LoginViewModel>();
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
            {
                _blockedAttempt++;
                DatabaseManager.Instance.Statistic.Update(_blockedAttempt);
                return;
            }

            CurrentlyLoggedVoter = voter;
            if (!CurrentlyLoggedVoter.Voted)
                ViewManager.ViewManager.Instance.OpenView<VoterViewModel>();
            else
                ViewManager.ViewManager.Instance.OpenView<StatisticsViewModel>();
        }

        public void LogOut()
        {
            CurrentlyLoggedVoter = null;
            ViewManager.ViewManager.Instance.OpenView<LoginViewModel>();
        }

        private static Report PrepCandReport(List<CandidateControl> cands)
        {
            var voters = DatabaseManager.Instance.Voter.Get();
            Report candReport = new Report(cands.ToReportSource());

            var valid = voters.Count(x => x.Voted && x.VoteValid);
            var invalid = voters.Count(x => x.Voted && !x.VoteValid);

            candReport.TextFields.Header = $"Votes valid/invalid - {valid}/{invalid} " + Environment.NewLine + $"Blocked attempts {DatabaseManager.Instance.Statistic.Get()}" + Environment.NewLine;
            candReport.TextFields.Title = "Candidates";
            candReport.DataFields["Vote"].Hidden = true;
            candReport.DataFields["InvalidVotes"].Hidden = true;
            candReport.DataFields["IsInDesignMode"].Hidden = true;
            return candReport;
        }

        private static Report PrepPartyReport(List<PartyControl> parts)
        {
            Report partiesReport = new Report(parts.ToReportSource());
            partiesReport.TextFields.Title = "Parties";
            partiesReport.DataFields["IsInDesignMode"].Hidden = true;
            partiesReport.DataFields["InvalidVotes"].Hidden = true;
            return partiesReport;
        }

        private async void _blockRefresher_ElapsedAsync(object sender, System.Timers.ElapsedEventArgs e)
        {
            await GetBlockedVotersAsync();
            _blockRefresher.Start();
        }

        private bool CheckVotersPrivilege(Voter voter, string strpesel)
        {
            //Check your privilege
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

        private async Task GetBlockedVotersAsync()
        {
            JToken blocked = await GetJsonBlockedAsync();
            _blockedPesels.Clear();
            foreach (var person in blocked)
            {
                _blockedPesels.Add(Encryptor.Hash(person.Value<string>("pesel")));
            }
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

        private async Task PopulateDatabaseAsync()
        {
            JToken candidates = await GetJsonCandidatesAsync();

            await Task.Factory.StartNew(() =>
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