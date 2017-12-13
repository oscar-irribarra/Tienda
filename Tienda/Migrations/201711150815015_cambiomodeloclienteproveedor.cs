namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiomodeloclienteproveedor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cliente", "Rut", c => c.String(nullable: false));
            AlterColumn("dbo.Proveedor", "Rut", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Proveedor", "Rut", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Cliente", "Rut", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
