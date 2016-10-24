using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VotesTotUp.Data.Helpers;
using VotesTotUp.Managers;
using static VotesTotUp.Data.Enum;

namespace VotesTotUp.Data.Database.Services
{
    public class VoterService
    {
        #region Fields

        private DbModelContainer _dbContext;
        private Encryption _encryption;

        #endregion Fields

        #region Constructors

        public VoterService(DbModelContainer db, Encryption encryption)
        {
            _dbContext = db;
            _encryption = encryption;
        }

        #endregion Constructors

        #region Methods

        public bool Add(Voter entity)
        {
            try
            {
                if (entity.FirstName == null || entity.LastName == null || entity.Pesel == null)
                    throw new Exception("Name and Pesel fields must not be null");

                entity.Pesel = _encryption.Hash(entity.Pesel);

                _dbContext.VoterSet.Add(entity);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return false;
            }
        }

        public Voter Get(string firstName, string lastName)
        {
            try
            {
                var voters = _dbContext.VoterSet.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
                var context = ((IObjectContextAdapter)_dbContext).ObjectContext;
                context.Refresh(RefreshMode.StoreWins, voters);

                return voters;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return null;
            }
        }

        public List<Voter> Get()
        {
            try
            {
                var voters = _dbContext.VoterSet.ToList();
                var context = ((IObjectContextAdapter)_dbContext).ObjectContext;
                context.Refresh(RefreshMode.StoreWins, voters);
                return voters;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return null;
            }
        }

        public Voter Get(string firstName, string lastName, string pesel, out Result result)
        {
            try
            {
                var hashPesel = _encryption.Hash(pesel);

                var voter = _dbContext.VoterSet.FirstOrDefault(x => x.Pesel == hashPesel && x.LastName == lastName && x.FirstName == firstName);

                if (voter == null)
                {
                    var voterByPesel = Get(pesel);
                    if (voterByPesel == null)
                    {
                        result = Result.DoesntExist;
                        return null;
                    }

                    result = Result.PeselInDb;
                    return null;
                }

                result = Result.Success;
                return voter;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                result = Result.Error;
                return null;
            }
        }

        public Voter Get(string pesel)
        {
            try
            {
                var hashPesel = _encryption.Hash(pesel);

                var voter = _dbContext.VoterSet.FirstOrDefault(x => x.Pesel == hashPesel);
                if (voter == null)
                    return null;

                voter.Candidates = _dbContext.CandidateSet.Where(x => x.Voters.FirstOrDefault(z => z.Id == voter.Id && z.Voted && z.VoteValid) != null).ToList();
                _dbContext.Entry(voter).State = System.Data.Entity.EntityState.Detached;
                return voter;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return null;
            }
        }

        public bool Remove(Voter entity)
        {
            try
            {
                _dbContext.VoterSet.Remove(entity);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return false;
            }
        }

        public bool Remove(int id)
        {
            try
            {
                var found = _dbContext.VoterSet.FirstOrDefault(x => x.Id == id);
                if (found == null)
                    throw new Exception($"DB:{ this.GetType().ToString()} entity with id {id} not found!");
                _dbContext.VoterSet.Remove(found);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return false;
            }
        }

        public bool Update(Voter entity)
        {
            try
            {
                if (entity.FirstName == null || entity.LastName == null || entity.Pesel == null)
                    throw new Exception("Name and Pesel fields must not be null");

                var found = _dbContext.VoterSet.FirstOrDefault(x => x.Id == entity.Id);
                var entry = _dbContext.Entry(found);

                if (found == null)
                    throw new Exception($"DB:{ this.GetType().ToString()} entity with id {entity.Id} not found!");

                found.Candidates = entity.Candidates;
                found.FirstName = entity.FirstName;
                found.LastName = entity.LastName;
                found.Pesel = entity.Pesel;
                found.Voted = entity.Voted;
                found.VoteValid = entity.VoteValid;

                _dbContext.VoterSet.Attach(found);

                entry.State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return false;
            }
        }

        #endregion Methods
    }
}