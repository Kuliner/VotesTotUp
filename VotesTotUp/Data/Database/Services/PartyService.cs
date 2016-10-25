using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VotesTotUp.Managers;

namespace VotesTotUp.Data.Database.Services
{
    public class PartyService
    {
        #region Fields

        private DbModelContainer _dbContext;
        private LogManager _logger;

        #endregion Fields

        #region Constructors

        public PartyService(DbModelContainer db, LogManager logger)
        {
            _dbContext = db;
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public bool Add(Party entity)
        {
            try
            {
                if (entity.Name == null)
                    throw new Exception("Name field must not be null");

                _dbContext.PartySet.Add(entity);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return false;
            }
        }

        public Party Get(string name)
        {
            try
            {
                return _dbContext.PartySet.FirstOrDefault(x => x.Name == name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public List<Party> Get()
        {
            try
            {
                var parties = _dbContext.PartySet.Include(nameof(Party.Candidates)).ToList();
                var context = ((IObjectContextAdapter)_dbContext).ObjectContext;
                context.Refresh(RefreshMode.StoreWins, parties);

                return parties;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public bool Remove(Party entity)
        {
            try
            {
                _dbContext.PartySet.Remove(entity);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return false;
            }
        }

        public bool Update(Party entity)
        {
            try
            {
                if (entity.Name == null)
                    throw new Exception("Name field must not be null");

                var found = _dbContext.PartySet.FirstOrDefault(x => x.Name == entity.Name);
                if (found == null)
                    throw new Exception($"DB: Party with id {entity.Name} not found!");
                found = entity;
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return false;
            }
        }

        #endregion Methods
    }
}