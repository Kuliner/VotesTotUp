using System;
using System.Collections.Generic;
using System.Linq;
using VotesTotUp.Managers;

namespace VotesTotUp.Data.Services
{
    public class CandidateService
    {
        #region Methods

        public bool Add(Candidate entity)
        {
            try
            {
                if (entity.Name == null || entity.Party == null)
                    throw new Exception("Name and Party fields must not be null");

                using (var context = new DbModelContainer())
                {
                    context.PartySet.Attach(entity.Party);
                    context.CandidateSet.Add(entity);
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

        public List<Candidate> Get()
        {
            try
            {
                using (var context = new DbModelContainer())
                {
                    return context.CandidateSet.Include(nameof(Candidate.Party)).Include(nameof(Candidate.Voters)).ToList();
                }
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
                using (var context = new DbModelContainer())
                {
                    return context.CandidateSet.FirstOrDefault(x => x.Name == name);
                }
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
                using (var context = new DbModelContainer())
                {
                    return context.CandidateSet.FirstOrDefault(x => x.Id == id);
                }
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
                using (var context = new DbModelContainer())
                {
                    var found = context.CandidateSet.FirstOrDefault(x => x.Id == id);
                    if (found == null)
                        throw new Exception($"DB:{ this.GetType().ToString()} entity with id {id} not found!");
                    context.CandidateSet.Remove(found);
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

        public bool Remove(Candidate entity)
        {
            try
            {
                using (var context = new DbModelContainer())
                {
                    context.CandidateSet.Remove(entity);
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

        public bool Update(Candidate entity)
        {
            try
            {
                if (entity.Name == null || entity.Party == null)
                    throw new Exception("Name and Party fields must not be null");

                using (var context = new DbModelContainer())
                {
                    var found = context.CandidateSet.FirstOrDefault(x => x.Id == entity.Id);
                    if (found == null)
                        throw new Exception($"DB:{ this.GetType().ToString()} entity with id {entity.Id} not found!");
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