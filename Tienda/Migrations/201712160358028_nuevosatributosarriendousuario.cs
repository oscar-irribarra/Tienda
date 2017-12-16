namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevosatributosarriendousuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arriendo", "Precio", c => c.Double(nullable: false));
            CreateIndex("dbo.AspNetUsers", "Rut", unique: true, name: "Usuario_Rut_Index");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", "Usuario_Rut_Index");
            DropColumn("dbo.Arriendo", "Precio");
        }
    }
}
