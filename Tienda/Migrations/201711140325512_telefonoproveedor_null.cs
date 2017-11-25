namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class telefonoproveedor_null : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Proveedor", "Telefono", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Proveedor", "Telefono", c => c.Int(nullable: false));
        }
    }
}
