namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quitarindiceunicoaspusers : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AspNetUsers", "Usuario_Rut_Index");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.AspNetUsers", "Rut", unique: true, name: "Usuario_Rut_Index");
        }
    }
}
