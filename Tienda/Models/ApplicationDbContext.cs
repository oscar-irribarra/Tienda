using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tienda.EntityConfigurations;

namespace Tienda.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
           : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<DetalleProducto> DetalleProductos { get; set; }
        public virtual DbSet<Inventario> Inventarios { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Impuesto> Impuestos { get; set; }

        //Compras
        public virtual DbSet<Adquisicion> Adquisiciones { get; set; }
        public virtual DbSet<DetalleAdquisicion> DetalleAdquisiciones { get; set; }
        public virtual DbSet<Proveedor> Proveedores { get; set; }

        //Ventas
        public virtual DbSet<Venta> Ventas { get; set; }
        public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }

        //Arriendo
        public virtual DbSet<Arriendo> Arriendos { get; set; }
        public virtual DbSet<DetalleArriendo> DetalleArriendos { get; set; }

        //Empresa
        public virtual DbSet<TipoProducto> TipoProductos { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }

        //Otros
        public virtual DbSet<Documento> Documentos { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<Contacto> Contactos { get; set; }



       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TipoProductoConfiguration());
            modelBuilder.Configurations.Add(new AdquisicionConfiguration());
            modelBuilder.Configurations.Add(new ArriendoConfiguration());        
            modelBuilder.Configurations.Add(new DetalleAdquisicionConfiguration());
            modelBuilder.Configurations.Add(new DetalleArriendoConfiguration());
            modelBuilder.Configurations.Add(new DetalleProductoConfiguration());
            modelBuilder.Configurations.Add(new DetalleVentaConfiguration());
            modelBuilder.Configurations.Add(new InventarioConfiguration());
            modelBuilder.Configurations.Add(new ContactoConfiguration());
            modelBuilder.Configurations.Add(new ProductoConfiguration());         
            modelBuilder.Configurations.Add(new VentaConfiguration());

            modelBuilder.Entity<Empresa>()
                .ToTable("Empresa")
                .HasKey(e => e.Id);            

            modelBuilder.Entity<Proveedor>()
                .ToTable("Proveedor")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Cliente>()
                .ToTable("Cliente")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Estado>()
                .ToTable("Estado")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Impuesto>()
                .ToTable("Impuesto")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Categoria>()
                .ToTable("Categoria")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Documento>()
                .ToTable("Documento")
                .HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }

}