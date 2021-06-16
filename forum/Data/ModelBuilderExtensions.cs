using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Data
{
    public static class ModelBuilderExtensions
    {

        public static void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var admin = new ApplicationUser
            {
                Id = "5d7570a2-cc6b-4953-b685-eb90db760143",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@test.com",
                NormalizedEmail = "admin@test.com",
                EmailConfirmed = false,
                SecurityStamp = new Guid().ToString("D")
            };

            admin.PasswordHash = PassGenerate(admin);

            builder.HasData(admin);
        }
        public static void Seed(this ModelBuilder modelBuilder)
        {
           

            //modelBuilder.Entity<ApplicationUser>().HasData(
            //    new ApplicationUser
            //    {
            //        Id= "5d7570a2-cc6b-4953-b685-eb90db760143",
            //        UserName = "admin",
            //        NormalizedUserName = "ADMIN",
            //        Email = "admin@test.com",
            //        NormalizedEmail = "admin@test.com",
            //        EmailConfirmed = false,
            //        SecurityStamp = new Guid().ToString("D"),
            //    }
            //);

        }
        public static string PassGenerate(ApplicationUser user)
        {
            var passHash = new PasswordHasher<ApplicationUser>();
            return passHash.HashPassword(user, "password");
        }
    }
}
