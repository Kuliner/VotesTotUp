using System;
using System.Collections.Generic;
using System.Linq;
using VotesTotUp.Managers;

namespace VotesTotUp.Data.Services
{
    public class VoterService
    {
        public bool Add(Voter entity)
        {
            try
            {
                if (entity.FirstName == null || entity.LastName == null || entity.Pesel == null)
                    throw new Exception("Name and Pesel fields must not be null");

                using (var context = new DbModelContainer())
                {
                    context.Database.Connection.Open();
                    context.VoterSet.Add(entity);
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

        public Voter Get(string firstName, string lastName)
        {
            try
            {
                using (var context = new DbModelContainer())
                {
                    context.Database.Connection.Open();
                    return context.VoterSet.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
                }
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
                using (var context = new DbModelContainer())
                {
                    context.Database.Connection.Open();
                    return context.VoterSet.ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(ex.StackTrace);
                return null;
            }
        }

        public Voter Get(int id)
        {
            try
            {
                using (var context = new DbModelContainer())
                {
                    context.Database.Connection.Open();
                    return context.VoterSet.FirstOrDefault(x => x.Id == id);
                }
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
                using (var context = new DbModelContainer())
                {
                    context.Database.Connection.Open();
                    context.VoterSet.Remove(entity);
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

        public bool Update(Voter entity)
        {
            try
            {
                if (entity.FirstName == null || entity.LastName == null || entity.Pesel == null)
                    throw new Exception("Name and Pesel fields must not be null");

                using (var context = new DbModelContainer())
                {
                    context.Database.Connection.Open();
                    var found = context.VoterSet.FirstOrDefault(x => x.Id == entity.Id);
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

        public bool Remove(int id)
        {
            try
            {
                using (var context = new DbModelContainer())
                {
                    context.Database.Connection.Open();
                    var found = context.VoterSet.FirstOrDefault(x => x.Id == id);
                    if (found == null)
                        throw new Exception($"DB:{ this.GetType().ToString()} entity with id {id} not found!");
                    context.VoterSet.Remove(found);
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
    }
}