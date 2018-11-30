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
            var usersToRemove = context.Users;
            foreach (var user in usersToRemove)
            {
                context.Users.Remove(user);
            }
            context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var users = new List<User>
            {
                new User()
                {
                    Username = "Patient1",
                    PasswordHash = "Password",
                    Role = "Patient",
                    Name = "PatientName1",
                    LastName = "PatientLastName1",
                    Pesel = "97030312542",
                },
                new User()
                {
                    Username = "Patient2",
                    PasswordHash = "Password",
                    Role = "Patient",
                    Name = "PatientName2",
                    LastName = "PatientLastName2",
                    Pesel = "97030312542",
                },
                new User()
                {
                    Username = "Doctor1",
                    PasswordHash = "Password",
                    Role = "Doctor",
                    Name = "DoctorName1",
                    LastName = "DoctorLastName1",
                    Pesel = "97102345721",
                },
                new User()
                {
                    Username = "Doctor2",
                    PasswordHash = "Password",
                    Role = "Doctor",
                    Name = "DoctorName1",
                    LastName = "DoctorLastName1",
                    Pesel = "98121312542",
                },
                new User()
                {
                    Username = "Pharmacist1",
                    PasswordHash = "Password",
                    Role = "Pharmacist",
                    Name = "PharmacistName1",
                    LastName = "PharmacistLastName1",
                    Pesel = "99100312567",
                },
                new User()
                {
                    Username = "Pharmacist2",
                    PasswordHash = "Password",
                    Role = "Pharmacist",
                    Name = "PharmacistName1",
                    LastName = "PharmacistLastName1",
                    Pesel = "96060346212",
                }
            };

            users.ForEach(u => context.Users.AddOrUpdate(u));
            context.SaveChanges();
        }
    }
}
