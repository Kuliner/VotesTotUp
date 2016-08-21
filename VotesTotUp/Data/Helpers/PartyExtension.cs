using System.Collections.Generic;

namespace VotesTotUp.Data.Helpers
{
    public static class PartyExtension
    {
        #region Methods

        public static List<PartyControl> ConvertToPartyControl(this List<Party> parties)
        {
            var pc = new List<PartyControl>();
            foreach (var party in parties)
            {
                var votes = 0;
                var invalidVotes = 0;
                foreach (var cand in party.Candidates)
                {
                    var candVotes = 0;
                    var candInvalidVotes = 0;
                    foreach (var voter in cand.Voters)
                    {
                        if (voter.Voted && voter.VoteValid)
                            candVotes++;
                        else if (voter.Voted && !voter.VoteValid)
                            candInvalidVotes++;
                    }

                    votes += candVotes;
                    invalidVotes += candInvalidVotes;
                }

                pc.Add(new PartyControl() { Name = party.Name, Votes = votes, InvalidVotes = invalidVotes });
            }

            return pc;
        }

        #endregion Methods
    }
}