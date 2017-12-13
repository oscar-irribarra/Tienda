namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiodenombretablacotizacion : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Cotizacions", newName: "Cotizacion");
            RenameTable(name: "dbo.DetalleCotizacions", newName: "DetalleCotizacion");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DetalleCotizacion", newName: "DetalleCotizacions");
            RenameTable(name: "dbo.Cotizacion", newName: "Cotizacions");
        }
    }
}
