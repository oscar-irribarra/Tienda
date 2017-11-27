using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tienda.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public static List<Roles> GetRoles()
        {
            var lista = new List<Roles>
            {
            new Roles{ Id = 0, Nombre = Rol.Usuario },
            new Roles{ Id = 1, Nombre = Rol.Admin },
            new Roles{ Id = 2, Nombre = Rol.Vendedor }
            };
            return lista.Where(x=>x.Id != 0).OrderBy(d => d.Nombre).ToList();
        }
    }
    static class Rol
    {
        public const string Admin = "Admin";
        public const string Vendedor = "Vendedor";
        public const string Usuario = "Usuario";

    }
}