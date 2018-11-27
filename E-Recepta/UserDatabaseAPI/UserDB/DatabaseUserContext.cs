using UserDatabaseAPI.UserDB.Entities;

namespace UserDatabaseAPI.UserDB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DatabaseUserContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }

        public const string connectionString =
            "data source=den1.mssql7.gear.host;" +
            "initial catalog=userdatabaseio;" +
            "persist security info=True;" +
            "user id=userdatabaseio;" +
            "password=Gd94sVZC!X-P;" +
            "MultipleActiveResultSets=True;" +
            "App=EntityFramework";
        public DatabaseUserContext()
            : base(connectionString)
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique(true);
        }
    }
}
