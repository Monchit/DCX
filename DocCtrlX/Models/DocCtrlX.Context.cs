﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DocCtrlX.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DocCtrlXEntities : DbContext
    {
        public DocCtrlXEntities()
            : base("name=DocCtrlXEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<td_distribution> td_distribution { get; set; }
        public DbSet<td_doc> td_doc { get; set; }
        public DbSet<td_tran> td_tran { get; set; }
        public DbSet<tm_doc_type> tm_doc_type { get; set; }
        public DbSet<tm_level> tm_level { get; set; }
    }
}
