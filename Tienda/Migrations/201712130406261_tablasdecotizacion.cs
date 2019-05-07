namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablasdecotizacion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cotizacions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Comentario = c.String(),
                        ClienteId = c.Int(nullable: false),
                        EstadoId = c.Int(nullable: false),
                        EmpresaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cliente", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId, cascadeDelete: true)
                .ForeignKey("dbo.Estado", t => t.EstadoId, cascadeDelete: true)
                .Index(t => t.ClienteId)
                .Index(t => t.EstadoId)
                .Index(t => t.EmpresaId);
            
            CreateTable(
                "dbo.DetalleCotizacions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CotizacionId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cotizacions", t => t.CotizacionId, cascadeDelete: true)
                .ForeignKey("dbo.Producto", t => t.ProductoId, cascadeDelete: true)
                .Index(t => t.CotizacionId)
                .Index(t => t.ProductoId);
            
            AlterColumn("dbo.Producto", "Nombre", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cotizacions", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Cotizacions", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.DetalleCotizacions", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.DetalleCotizacions", "CotizacionId", "dbo.Cotizacions");
            DropForeignKey("dbo.Cotizacions", "ClienteId", "dbo.Cliente");
            DropIndex("dbo.DetalleCotizacions", new[] { "ProductoId" });
            DropIndex("dbo.DetalleCotizacions", new[] { "CotizacionId" });
            DropIndex("dbo.Cotizacions", new[] { "EmpresaId" });
            DropIndex("dbo.Cotizacions", new[] { "EstadoId" });
            DropIndex("dbo.Cotizacions", new[] { "ClienteId" });
            AlterColumn("dbo.Producto", "Nombre", c => c.String(nullable: false, maxLength: 100));
            DropTable("dbo.DetalleCotizacions");
            DropTable("dbo.Cotizacions");
        }
    }
}
