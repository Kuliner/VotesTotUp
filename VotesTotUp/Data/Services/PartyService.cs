using System;
using System.Collections.Generic;
using System.Linq;
using VotesTotUp.Managers;

namespace VotesTotUp.Data.Services
{
    public class PartyService
    {
        #region Methods

        public bool Add(Party entity)
        {
            try
            {
                if (entity.Name == null)
                    throw new Exception("Name field must not be null");

                using (var context = new DbModelContainer())
                {
                    context.PartySet.Add(entity);
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return false;
            }
        }

        public Party Get(string name)
        {
            try
            {
                using (var context = new DbModelContainer())
                {
                    return context.PartySet.FirstOrDefault(x => x.Name == name);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return null;
            }
        }

        public List<Party> Get()
        {
            try
            {
                using (var context = new DbModelContainer())
                {
                    return context.PartySet.Include(nameof(Party.Candidates)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return null;
            }
        }

        public bool Remove(Party entity)
        {
            try
            {
                using (var context = new DbModelContainer())
                {
                    context.PartySet.Remove(entity);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return false;
            }
        }

        public bool Update(Party entity)
        {
            try
            {
                if (entity.Name == null)
                    throw new Exception("Name field must not be null");

                using (var context = new DbModelContainer())
                {
                    var found = context.PartySet.FirstOrDefault(x => x.Name == entity.Name);
                    if (found == null)
                        throw new Exception($"DB: Party with id {entity.Name} not found!");
                    found = entity;
                    context.SaveChanges();
                }

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