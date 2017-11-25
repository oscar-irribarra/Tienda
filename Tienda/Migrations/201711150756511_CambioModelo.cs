namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambioModelo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cliente", "Rut", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("dbo.Proveedor", "Rut", c => c.String(nullable: false, maxLength: 13));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Proveedor", "Rut", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Cliente", "Rut", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
