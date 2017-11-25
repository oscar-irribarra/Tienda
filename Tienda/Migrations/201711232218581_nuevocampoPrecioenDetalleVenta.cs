namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevocampoPrecioenDetalleVenta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetalleVenta", "Precio", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetalleVenta", "Precio");
        }
    }
}
