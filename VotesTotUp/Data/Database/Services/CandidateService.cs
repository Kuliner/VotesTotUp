using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VotesTotUp.Managers;

namespace VotesTotUp.Data.Database.Services
{
    public class CandidateService
    {
        #region Fields

        private DbModelContainer _dbContext;

        #endregion Fields

        #region Constructors

        public CandidateService(DbModelContainer db)
        {
            _dbContext = db;
        }

        #endregion Constructors

        #region Methods

        public bool Add(Candidate entity)
        {
            try
            {
                if (entity.Name == null || entity.Party == null)
                    throw new Exception("Name and Party fields must not be null");

                _dbContext.PartySet.Attach(entity.Party);
                _dbContext.CandidateSet.Add(entity);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return false;
            }
        }

        public List<Candidate> Get()
        {
            try
            {
                var cands = _dbContext.CandidateSet.Include(nameof(Candidate.Party)).Include(nameof(Candidate.Voters)).ToList();

                var context = ((IObjectContextAdapter)_dbContext).ObjectContext;
                context.Refresh(RefreshMode.StoreWins, cands);

                cands = _dbContext.CandidateSet.Include(nameof(Candidate.Party)).Include(nameof(Candidate.Voters)).ToList();

                return cands;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return null;
            }
        }

        public Candidate Get(string name)
        {
            try
            {
                var context = ((IObjectContextAdapter)_dbContext).ObjectContext;
                context.Refresh(RefreshMode.StoreWins, context.ObjectStateManager.GetObjectStateEntries(System.Data.Entity.EntityState.Modified));

                return _dbContext.CandidateSet.FirstOrDefault(x => x.Name == name);
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return null;
            }
        }

        public Candidate Get(int id)
        {
            try
            {
                var context = ((IObjectContextAdapter)_dbContext).ObjectContext;
                context.Refresh(RefreshMode.StoreWins, context.ObjectStateManager.GetObjectStateEntries(System.Data.Entity.EntityState.Modified));

                return _dbContext.CandidateSet.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return null;
            }
        }

        public bool Remove(int id)
        {
            try
            {
                var found = _dbContext.CandidateSet.FirstOrDefault(x => x.Id == id);
                if (found == null)
                    throw new Exception($"DB:{ this.GetType().ToString()} entity with id {id} not found!");
                _dbContext.CandidateSet.Remove(found);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return false;
            }
        }

        public bool Remove(Candidate entity)
        {
            try
            {
                _dbContext.CandidateSet.Remove(entity);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return false;
            }
        }

        public bool Update(Candidate entity)
        {
            try
            {
                if (entity.Name == null || entity.Party == null)
                    throw new Exception("Name and Party fields must not be null");

                var found = _dbContext.CandidateSet.FirstOrDefault(x => x.Id == entity.Id);
                if (found == null)
                    throw new Exception($"DB:{ this.GetType().ToString()} entity with id {entity.Id} not found!");
                found = entity;
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