using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Hydro_Mobil.Migrations;

namespace Hydro_Mobil.Models
{
    public class TablolarContext : DbContext
    {
        public TablolarContext() : base("ConConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TablolarContext, Configuration>("ConConnectionString"));

        }
        public DbSet<Members> Member { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}