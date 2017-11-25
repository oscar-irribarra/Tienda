namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tablas_Negocio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adquisicion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        DocumentoId = c.Int(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                        EstadoId = c.Int(nullable: false),
                        EmpresaId = c.Int(nullable: false),
                        TipoProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documento", t => t.DocumentoId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Proveedor", t => t.ProveedorId)
                .ForeignKey("dbo.TipoProducto", t => t.TipoProductoId)
                .Index(t => t.DocumentoId)
                .Index(t => t.ProveedorId)
                .Index(t => t.EstadoId)
                .Index(t => t.EmpresaId)
                .Index(t => t.TipoProductoId);
            
            CreateTable(
                "dbo.DetalleAdquisicion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdquisicionId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adquisicion", t => t.AdquisicionId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.AdquisicionId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Precio = c.Double(nullable: false),
                        Barcode = c.Int(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                        ImpuestoId = c.Int(nullable: false),
                        TipoProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Impuesto", t => t.ImpuestoId)
                .ForeignKey("dbo.TipoProducto", t => t.TipoProductoId)
                .Index(t => t.CategoriaId)
                .Index(t => t.ImpuestoId)
                .Index(t => t.TipoProductoId);
            
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        TipoProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoProducto", t => t.TipoProductoId, cascadeDelete: true)
                .Index(t => t.TipoProductoId);
            
            CreateTable(
                "dbo.TipoProducto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.Int(nullable: false),
                        Nombre = c.String(),
                        AfectaIva = c.Boolean(nullable: false),
                        EmpresaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .Index(t => t.EmpresaId);
            
            CreateTable(
                "dbo.Arriendo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(nullable: false),
                        DocumentoId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        EstadoId = c.Int(nullable: false),
                        EmpresaId = c.Int(nullable: false),
                        TipoProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.Documento", t => t.DocumentoId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.TipoProducto", t => t.TipoProductoId)
                .Index(t => t.DocumentoId)
                .Index(t => t.ClienteId)
                .Index(t => t.EstadoId)
                .Index(t => t.EmpresaId)
                .Index(t => t.TipoProductoId);
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rut = c.String(nullable: false, maxLength: 10),
                        RazonSocial = c.String(nullable: false, maxLength: 100),
                        Giro = c.String(nullable: false, maxLength: 100),
                        Comuna = c.String(nullable: false, maxLength: 100),
                        Ciudad = c.String(nullable: false, maxLength: 100),
                        Telefono = c.Int(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Venta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        DocumentoId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        EmpresaId = c.Int(nullable: false),
                        EstadoId = c.Int(nullable: false),
                        TipoProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.Documento", t => t.DocumentoId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.TipoProducto", t => t.TipoProductoId)
                .Index(t => t.DocumentoId)
                .Index(t => t.ClienteId)
                .Index(t => t.EmpresaId)
                .Index(t => t.EstadoId)
                .Index(t => t.TipoProductoId);
            
            CreateTable(
                "dbo.DetalleVenta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VentaId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .ForeignKey("dbo.Venta", t => t.VentaId)
                .Index(t => t.VentaId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Documento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Codigo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Empresa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rut = c.String(nullable: false, maxLength: 10),
                        RazonSocial = c.String(nullable: false, maxLength: 100),
                        Giro = c.String(nullable: false, maxLength: 100),
                        Direccion = c.String(nullable: false, maxLength: 100),
                        Comuna = c.String(nullable: false, maxLength: 100),
                        Ciudad = c.String(nullable: false, maxLength: 100),
                        RepresentanteLegal = c.String(nullable: false, maxLength: 150),
                        Telefono = c.Int(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mensaje",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Telefono = c.Int(nullable: false),
                        Correo = c.String(nullable: false),
                        Contenido = c.String(nullable: false, maxLength: 350),
                        EstadoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .Index(t => t.EstadoId);
            
            CreateTable(
                "dbo.DetalleArriendo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArriendoId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Arriendo", t => t.ArriendoId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.ArriendoId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.DetalleProducto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Marca = c.String(),
                        Modelo = c.String(),
                        Color = c.String(),
                        Descripcion = c.String(),
                        ImagenUrl = c.String(),
                        ProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Impuesto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Tasa = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inventario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductoId = c.Int(nullable: false),
                        Stock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rut = c.String(nullable: false, maxLength: 15),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Correo = c.String(maxLength: 100),
                        Telefono = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Nombre", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.AspNetUsers", "Apellido", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adquisicion", "TipoProductoId", "dbo.TipoProducto");
            DropForeignKey("dbo.Adquisicion", "ProveedorId", "dbo.Proveedor");
            DropForeignKey("dbo.Adquisicion", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Adquisicion", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Adquisicion", "DocumentoId", "dbo.Documento");
            DropForeignKey("dbo.DetalleAdquisicion", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Producto", "TipoProductoId", "dbo.TipoProducto");
            DropForeignKey("dbo.Inventario", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Producto", "ImpuestoId", "dbo.Impuesto");
            DropForeignKey("dbo.DetalleProducto", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Producto", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.TipoProducto", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Categoria", "TipoProductoId", "dbo.TipoProducto");
            DropForeignKey("dbo.Arriendo", "TipoProductoId", "dbo.TipoProducto");
            DropForeignKey("dbo.Arriendo", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Arriendo", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Arriendo", "DocumentoId", "dbo.Documento");
            DropForeignKey("dbo.DetalleArriendo", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.DetalleArriendo", "ArriendoId", "dbo.Arriendo");
            DropForeignKey("dbo.Arriendo", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Venta", "TipoProductoId", "dbo.TipoProducto");
            DropForeignKey("dbo.Venta", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Mensaje", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Venta", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Venta", "DocumentoId", "dbo.Documento");
            DropForeignKey("dbo.DetalleVenta", "VentaId", "dbo.Venta");
            DropForeignKey("dbo.DetalleVenta", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Venta", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.DetalleAdquisicion", "AdquisicionId", "dbo.Adquisicion");
            DropIndex("dbo.Inventario", new[] { "ProductoId" });
            DropIndex("dbo.DetalleProducto", new[] { "ProductoId" });
            DropIndex("dbo.DetalleArriendo", new[] { "ProductoId" });
            DropIndex("dbo.DetalleArriendo", new[] { "ArriendoId" });
            DropIndex("dbo.Mensaje", new[] { "EstadoId" });
            DropIndex("dbo.DetalleVenta", new[] { "ProductoId" });
            DropIndex("dbo.DetalleVenta", new[] { "VentaId" });
            DropIndex("dbo.Venta", new[] { "TipoProductoId" });
            DropIndex("dbo.Venta", new[] { "EstadoId" });
            DropIndex("dbo.Venta", new[] { "EmpresaId" });
            DropIndex("dbo.Venta", new[] { "ClienteId" });
            DropIndex("dbo.Venta", new[] { "DocumentoId" });
            DropIndex("dbo.Arriendo", new[] { "TipoProductoId" });
            DropIndex("dbo.Arriendo", new[] { "EmpresaId" });
            DropIndex("dbo.Arriendo", new[] { "EstadoId" });
            DropIndex("dbo.Arriendo", new[] { "ClienteId" });
            DropIndex("dbo.Arriendo", new[] { "DocumentoId" });
            DropIndex("dbo.TipoProducto", new[] { "EmpresaId" });
            DropIndex("dbo.Categoria", new[] { "TipoProductoId" });
            DropIndex("dbo.Producto", new[] { "TipoProductoId" });
            DropIndex("dbo.Producto", new[] { "ImpuestoId" });
            DropIndex("dbo.Producto", new[] { "CategoriaId" });
            DropIndex("dbo.DetalleAdquisicion", new[] { "ProductoId" });
            DropIndex("dbo.DetalleAdquisicion", new[] { "AdquisicionId" });
            DropIndex("dbo.Adquisicion", new[] { "TipoProductoId" });
            DropIndex("dbo.Adquisicion", new[] { "EmpresaId" });
            DropIndex("dbo.Adquisicion", new[] { "EstadoId" });
            DropIndex("dbo.Adquisicion", new[] { "ProveedorId" });
            DropIndex("dbo.Adquisicion", new[] { "DocumentoId" });
            DropColumn("dbo.AspNetUsers", "Apellido");
            DropColumn("dbo.AspNetUsers", "Nombre");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Inventario");
            DropTable("dbo.Impuesto");
            DropTable("dbo.DetalleProducto");
            DropTable("dbo.DetalleArriendo");
            DropTable("dbo.Mensaje");
            DropTable("dbo.Estado");
            DropTable("dbo.Empresa");
            DropTable("dbo.Documento");
            DropTable("dbo.DetalleVenta");
            DropTable("dbo.Venta");
            DropTable("dbo.Cliente");
            DropTable("dbo.Arriendo");
            DropTable("dbo.TipoProducto");
            DropTable("dbo.Categoria");
            DropTable("dbo.Producto");
            DropTable("dbo.DetalleAdquisicion");
            DropTable("dbo.Adquisicion");
        }
    }
}
