namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiodenombreMensajeContacto : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Mensaje", newName: "Contacto");
            AddColumn("dbo.Contacto", "Nombre", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacto", "Nombre");
            RenameTable(name: "dbo.Contacto", newName: "Mensaje");
        }
    }
}
