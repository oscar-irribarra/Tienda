namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NuevosatributosTablaMensaje : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mensaje", "Ip", c => c.String());
            AlterColumn("dbo.Mensaje", "Telefono", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Mensaje", "Telefono", c => c.Int(nullable: false));
            DropColumn("dbo.Mensaje", "Ip");
        }
    }
}
