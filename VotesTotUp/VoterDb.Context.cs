﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VotesTotUp
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DbModelContainer : DbContext
    {
        public DbModelContainer()
            : base("name=DbModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Candidate> CandidateSet { get; set; }
        public virtual DbSet<Party> PartySet { get; set; }
        public virtual DbSet<Voter> VoterSet { get; set; }
        public virtual DbSet<Statistics> Statistics { get; set; }
    }
}
