using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Tienda.Models;
using System.Data.Entity;

namespace Tienda.Helpers
{
    public class RootHelper
    {
        private static ApplicationDbContext context = new ApplicationDbContext();

        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var email = SEmpresa.email;
           
            var rol = Rol.Admin;
            var userASP = userManager.FindByName(email);
            if (userASP == null)
            {
                CreateUserASP(email, rol, email);
                return;
            }
            userManager.AddToRole(userASP.Id, rol);
        }
      
        public static void CreateUserASP(string email, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var userASP = new ApplicationUser
            {
                Apellido = "Admin",
                Nombre = "Admin",
                EmailConfirmed = true,
                Rut = "1",
                Isbloqued = false,
                Email = email,
                UserName = email,
            };

            userManager.Create(userASP, password);
            userManager.AddToRole(userASP.Id, roleName);
        }

        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                      
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }

    }
}