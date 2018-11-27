using System.Collections.Generic;
using UserDatabaseAPI.UserDB.Entities;

namespace UserDatabaseAPI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<UserDatabaseAPI.UserDB.DatabaseUserContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(UserDatabaseAPI.UserDB.DatabaseUserContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var users = new List<User>
            {
                new User()
                {
                    Username = "Patient1",
                    PasswordHash = "Password",
                    Role = "Patient"
                },
                new User()
                {
                    Username = "Patient2",
                    PasswordHash = "Password",
                    Role = "Patient"
                },
                new User()
                {
                    Username = "Doctor1",
                    PasswordHash = "Password",
                    Role = "Doctor"
                },
                new User()
                {
                    Username = "Doctor2",
                    PasswordHash = "Password",
                    Role = "Doctor"
                },
                new User()
                {
                    Username = "Pharmacist1",
                    PasswordHash = "Password",
                    Role = "Pharmacist"
                },
                new User()
                {
                    Username = "Pharmacist2",
                    PasswordHash = "Password",
                    Role = "Pharmacist"
                }
            };
            users.ForEach(u=> context.Users.Add(u));
            context.SaveChanges();
        }
    }
}
