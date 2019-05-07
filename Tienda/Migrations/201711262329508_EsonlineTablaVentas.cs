namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EsonlineTablaVentas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Venta", "EsOnline", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Venta", "EsOnline");
        }
    }
}
