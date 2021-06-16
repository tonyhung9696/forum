using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Data
{
    public class AdminConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var admin = new ApplicationUser
            {
                Id = "5d7570a2-cc6b-4953-b685-eb90db760143",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@test.com",
                NormalizedEmail = "admin@test.com",
                EmailConfirmed = false,
                SecurityStamp = new Guid().ToString("D"),
            };
            admin.PasswordHash = PassGenerate(admin);
            builder.HasData(admin);

            var vistor = new ApplicationUser
            {
                Id = "61eaa57f-62be-48d1-9889-923571d4c785",
                UserName = "vistor",
                NormalizedUserName = "VISTOR",
                Email = "vistor@test.com",
                NormalizedEmail = "vistor@test.com",
                EmailConfirmed = false,
                SecurityStamp = new Guid().ToString("D"),
            };
            vistor.PasswordHash = PassGenerate(vistor);
            builder.HasData(vistor);
        }

        public string PassGenerate(ApplicationUser user)
        {
            var passHash = new PasswordHasher<ApplicationUser>();
            return passHash.HashPassword(user, "password");
        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        private const string visitorId = "34ae21f5-9cbf-4883-9667-919b144405f2";
        private const string adminId = "ba776e03-6c4a-4f52-bdb4-ed99a51cb69d";
        private const string visitorCS = "94d90dfe-c01f-4148-aa35-9a4d5da3e483";
        private const string adminCS = "fb111548-aa04-4514-ad1b-943e78dc1fd3";

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {

            builder.HasData(
                    new IdentityRole
                    {
                        Id = visitorId,
                        Name = "Visitor",
                        NormalizedName = "VISITOR",
                        ConcurrencyStamp=visitorCS
                    },
                    new IdentityRole
                    {
                        Id = adminId,
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp=adminCS
                    }
                );
        }
    }
    public class UsersWithRolesConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        private const string visitorRoleId = "34ae21f5-9cbf-4883-9667-919b144405f2";
        private const string adminRoleId = "ba776e03-6c4a-4f52-bdb4-ed99a51cb69d";

        private const string adminUserId = "5d7570a2-cc6b-4953-b685-eb90db760143";
        private const string visitorUserId = "61eaa57f-62be-48d1-9889-923571d4c785";

        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {

            builder.HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = adminRoleId,
                        UserId = adminUserId
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = visitorRoleId,
                        UserId = adminUserId
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = visitorRoleId,
                        UserId = visitorUserId
                    }
                );

        }
    }
}
