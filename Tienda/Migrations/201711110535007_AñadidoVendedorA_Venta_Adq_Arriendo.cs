namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AñadidoVendedorA_Venta_Adq_Arriendo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Adquisicion", "VendedorId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Arriendo", "VendedorId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Venta", "VendedorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Adquisicion", "VendedorId");
            CreateIndex("dbo.Arriendo", "VendedorId");
            CreateIndex("dbo.Venta", "VendedorId");
            AddForeignKey("dbo.Venta", "VendedorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Arriendo", "VendedorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Adquisicion", "VendedorId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adquisicion", "VendedorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Arriendo", "VendedorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Venta", "VendedorId", "dbo.AspNetUsers");
            DropIndex("dbo.Venta", new[] { "VendedorId" });
            DropIndex("dbo.Arriendo", new[] { "VendedorId" });
            DropIndex("dbo.Adquisicion", new[] { "VendedorId" });
            DropColumn("dbo.Venta", "VendedorId");
            DropColumn("dbo.Arriendo", "VendedorId");
            DropColumn("dbo.Adquisicion", "VendedorId");
        }
    }
}
