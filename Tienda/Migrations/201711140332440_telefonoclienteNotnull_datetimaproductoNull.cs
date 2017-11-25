namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class telefonoclienteNotnull_datetimaproductoNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Producto", "FechaVencimiento", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Producto", "FechaVencimiento", c => c.DateTime(nullable: false));
        }
    }
}
