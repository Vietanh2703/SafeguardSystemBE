using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //RoleID
            var guardRoleId = Guid.Parse("D1616B66-90CC-479F-B45E-1E86378937F7");
            var adminRoleId = Guid.Parse("7A04E1D4-C176-467D-AC7D-6E1433CE6F3E");
            var managerRoleId = Guid.Parse("6BE95231-36AA-4A26-8C61-B65E040EC32A");
            var businessRoleId = Guid.Parse("BE19E4B3-6664-4AFD-9EBB-98E0A073EDC9");

            //UserID
            var anhId = "kRw3cZIvwwZC4wDN8SJN9lrEbwP2";
            var namId = "ukwN487LifQFzMK51XkNVbqsfXB2";
            

            //Role seed
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = guardRoleId,
                    RoleName = "Guard",
                },
                new Role
                {
                    RoleId = adminRoleId,
                    RoleName = "Admin",
                },
                new Role
                {
                    RoleId = managerRoleId,
                    RoleName = "Manager",
                },
                new Role
                {
                    RoleId = businessRoleId,
                    RoleName = "Business partner",
                }
            );

            //User role seed
            //All user password is 12345
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = anhId,
                    UserName = "Admin",
                    Email = "admin@test.com",
                    FullName = "Viet Anh",
                    Avatar = "https://www.freepik.com/free-vector/simple-vibing-cat-square-meme_58459053.htm#fromView=keyword&page=1&position=0&uuid=f4bd18ef-8de6-4b6e-8e68-06073abf526b&query=Animal+Memes",
                    Phone = "0123456789",
                    BirthDay = new DateTime(2004, 3, 27),
                    IsActive = true,
                    IsEmailConfirmed = true,
                    IsDeleted = false,
                    RoleID = adminRoleId
                },
                new User
                {
                    UserId = namId,
                    UserName = "Manager",
                    Email = "manager@test.com",
                    FullName = "Nhat Nam",
                    Avatar = "https://www.freepik.com/free-vector/simple-vibing-cat-square-meme_58459053.htm#fromView=keyword&page=1&position=0&uuid=f4bd18ef-8de6-4b6e-8e68-06073abf526b&query=Animal+Memes",
                    Phone = "0123456789",
                    BirthDay = new DateTime(2004, 1, 1),
                    IsActive = true,
                    IsEmailConfirmed = true,
                    IsDeleted = false,
                    RoleID = managerRoleId
                }


            );
        }
    }
}
