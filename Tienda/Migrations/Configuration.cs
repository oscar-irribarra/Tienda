namespace Tienda.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Tienda.Models;
    internal sealed class Configuration : DbMigrationsConfiguration<Tienda.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Tienda.Models.ApplicationDbContext context)
        {
            context.Empresas.AddOrUpdate(e => e.Rut,
                new Empresa()
                {
                    
                    Rut = "99999999-9",
                    RazonSocial = "Sociedad de Comercio y Telecomunicaciones Limitada",
                    Giro = "Insumos Basicos y Seguridad",
                    Direccion = "24 1/2 Ote ; 20 1/2 Norte 3321",
                    Comuna = "Talca",
                    Ciudad = "Talca",
                    RepresentanteLegal = "Juan",
                    Telefono = 0,
                    Email = "sostel@sostel.cl",
                    Id = 1
                    
                });

            context.TipoProductos.AddOrUpdate(a => a.Nombre,
                    new TipoProducto { Id = 1, Nombre = "Venta de Abarrotes", EmpresaId = 1 },
                    new TipoProducto { Id = 2, Nombre = "Seguridad y Telecomunicaciones", EmpresaId = 1 }
                    );


            context.Documentos.AddOrUpdate(d => d.Nombre,
                new Documento { Id = 1, Nombre = TipoDocumento.Factura, Codigo = 1 },
                new Documento { Id = 2, Nombre = TipoDocumento.Boleta, Codigo = 2 }
                );

            context.Estados.AddOrUpdate(e => e.Nombre,
                new Estado { Id = 1, Nombre = TipoEstado.Listo },
                new Estado { Id = 2, Nombre = TipoEstado.Cancelado },
                new Estado { Id = 3, Nombre = TipoEstado.Finalizado },
                new Estado { Id = 4, Nombre = TipoEstado.EnCurso }
                );

            context.Impuestos.AddOrUpdate(i => i.Nombre,
                new Impuesto { Id = 1, Nombre = "19%", Tasa = 19 }
                );
                     
            context.Clientes.AddOrUpdate(x => x.Id,
                new Cliente { Id = 1, Rut = "0", Telefono = "0", Nombre = "Venta a Publico", Apellido = "!", Email = "S/E"}
                );

            //context.Users.AddOrUpdate(x => x.Id,
            //   new ApplicationUser
            //   {
            //       Email = "",
            //       EmailConfirmed = true,
            //       PasswordHash = "AIHFWNlOPw1xxIAu0bax1vd5qITpFyQJVSyN0PjHwL/B9ELF6qUWJIB59WmpQ9T1lg==",
            //       SecurityStamp = "7935c1b4-ebed-4c2d-825a-8a4462531847",
            //       PhoneNumber = "",
            //       PhoneNumberConfirmed = false,
            //       TwoFactorEnabled = false,
            //       LockoutEnabled = false,
            //       LockoutEndDateUtc = null,
            //       AccessFailedCount = 0,
            //       UserName = "Venta Online",
            //       Nombre = "Venta",
            //       Apellido = "Online",
            //       Rut = "0",
            //       Isbloqued = false
            //   }
            //   );

        }
    }
}
