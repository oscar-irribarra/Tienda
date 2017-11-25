namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregadaFechaVencimiento_Productos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto", "FechaVencimiento", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producto", "FechaVencimiento");
        }
    }
}
