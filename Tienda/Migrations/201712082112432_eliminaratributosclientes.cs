namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eliminaratributosclientes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cliente", "Telefono", c => c.String(nullable: false));
            AlterColumn("dbo.Proveedor", "Telefono", c => c.String());
            DropColumn("dbo.Cliente", "Comuna");
            DropColumn("dbo.Cliente", "Ciudad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cliente", "Ciudad", c => c.String(maxLength: 100));
            AddColumn("dbo.Cliente", "Comuna", c => c.String(maxLength: 100));
            AlterColumn("dbo.Proveedor", "Telefono", c => c.Int());
            AlterColumn("dbo.Cliente", "Telefono", c => c.Int(nullable: false));
        }
    }
}
