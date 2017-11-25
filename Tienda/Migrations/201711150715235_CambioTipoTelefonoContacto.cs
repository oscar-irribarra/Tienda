namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambioTipoTelefonoContacto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacto", "Telefono", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacto", "Telefono", c => c.Int());
        }
    }
}
