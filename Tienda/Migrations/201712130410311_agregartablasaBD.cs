namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agregartablasaBD : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DetalleCotizacion", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Cotizacion", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.DetalleCotizacion", "CotizacionId", "dbo.Cotizacion");
            DropForeignKey("dbo.Cotizacion", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Cotizacion", "EstadoId", "dbo.Estado");
            AddForeignKey("dbo.DetalleCotizacion", "ProductoId", "dbo.Producto", "Id");
            AddForeignKey("dbo.Cotizacion", "ClienteId", "dbo.Cliente", "Id");
            AddForeignKey("dbo.DetalleCotizacion", "CotizacionId", "dbo.Cotizacion", "Id");
            AddForeignKey("dbo.Cotizacion", "EmpresaId", "dbo.Empresa", "Id");
            AddForeignKey("dbo.Cotizacion", "EstadoId", "dbo.Estado", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cotizacion", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Cotizacion", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.DetalleCotizacion", "CotizacionId", "dbo.Cotizacion");
            DropForeignKey("dbo.Cotizacion", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.DetalleCotizacion", "ProductoId", "dbo.Producto");
            AddForeignKey("dbo.Cotizacion", "EstadoId", "dbo.Estado", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Cotizacion", "EmpresaId", "dbo.Empresa", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DetalleCotizacion", "CotizacionId", "dbo.Cotizacion", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Cotizacion", "ClienteId", "dbo.Cliente", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DetalleCotizacion", "ProductoId", "dbo.Producto", "Id", cascadeDelete: true);
        }
    }
}
