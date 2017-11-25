using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tienda.ViewModels
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
    }

    public class RolesViewModel
    {
        public IEnumerable<string> RoleNames { get; set; }
        public string Rut { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool Isbloqued { get; set; }
    }
}