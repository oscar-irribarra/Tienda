namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AñadidoRut_Isbloqued_Usuarios : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Rut", c => c.String(nullable: false, maxLength: 15));
            AddColumn("dbo.AspNetUsers", "Isbloqued", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Isbloqued");
            DropColumn("dbo.AspNetUsers", "Rut");
        }
    }
}
