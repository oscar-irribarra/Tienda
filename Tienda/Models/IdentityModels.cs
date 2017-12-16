using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System;
using System.Collections.Generic;

namespace Tienda.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "El campo {0} es olbligatorio")]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener menos de {1} Caracteres")]
        [Index("Usuario_Rut_Index", IsUnique = true)]
        public string Rut { get; set; }
        [Required(ErrorMessage = "El campo {0} es olbligatorio")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener menos de {1} Caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es olbligatorio")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener menos de {1} Caracteres")]
        public string Apellido { get; set; }

        public bool Isbloqued { get; set; }

        public string FullName { get {  return string.Format("{0} {1}", Nombre, Apellido); } }

        public virtual ICollection<Venta> Venta { get; set; }
        public virtual ICollection<Adquisicion> Adquisicion { get; set; }
        public virtual ICollection<Arriendo> Arriendo { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            userIdentity.AddClaim(new Claim("FullName", FullName));
            return userIdentity;
        }

    }
    public static class IdentityExtensions
    {
        private static string GetDisplayName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                return ci.FindFirst("FullName").ToString().Substring(("FullName: ").Length); 
            }
            return null;
        }
    }




}