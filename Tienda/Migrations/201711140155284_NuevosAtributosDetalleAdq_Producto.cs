namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NuevosAtributosDetalleAdq_Producto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetalleAdquisicion", "Precio", c => c.Double(nullable: false));
            AddColumn("dbo.Cliente", "Nombre", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Cliente", "Apellido", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Cliente", "Comuna", c => c.String(maxLength: 100));
            AlterColumn("dbo.Cliente", "Ciudad", c => c.String(maxLength: 100));
            AlterColumn("dbo.Cliente", "Email", c => c.String(maxLength: 100));
            DropColumn("dbo.TipoProducto", "Codigo");
            DropColumn("dbo.TipoProducto", "AfectaIva");
            DropColumn("dbo.Cliente", "RazonSocial");
            DropColumn("dbo.Cliente", "Giro");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cliente", "Giro", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Cliente", "RazonSocial", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.TipoProducto", "AfectaIva", c => c.Boolean(nullable: false));
            AddColumn("dbo.TipoProducto", "Codigo", c => c.Int(nullable: false));
            AlterColumn("dbo.Cliente", "Email", c => c.String());
            AlterColumn("dbo.Cliente", "Ciudad", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Cliente", "Comuna", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Cliente", "Apellido");
            DropColumn("dbo.Cliente", "Nombre");
            DropColumn("dbo.DetalleAdquisicion", "Precio");
        }
    }
}
