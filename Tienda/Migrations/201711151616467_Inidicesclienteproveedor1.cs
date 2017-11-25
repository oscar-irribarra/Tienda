namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inidicesclienteproveedor1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Cliente", "Cliente_Rut_Index");
            DropIndex("dbo.Proveedor", "Proveedor_Rut_Index");
            AlterColumn("dbo.Cliente", "Rut", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("dbo.Proveedor", "Rut", c => c.String(nullable: false, maxLength: 12));
            CreateIndex("dbo.Cliente", "Rut", unique: true, name: "Cliente_Rut_Index");
            CreateIndex("dbo.Proveedor", "Rut", unique: true, name: "Proveedor_Rut_Index");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Proveedor", "Proveedor_Rut_Index");
            DropIndex("dbo.Cliente", "Cliente_Rut_Index");
            AlterColumn("dbo.Proveedor", "Rut", c => c.String(nullable: false, maxLength: 13));
            AlterColumn("dbo.Cliente", "Rut", c => c.String(nullable: false, maxLength: 13));
            CreateIndex("dbo.Proveedor", "Rut", unique: true, name: "Proveedor_Rut_Index");
            CreateIndex("dbo.Cliente", "Rut", unique: true, name: "Cliente_Rut_Index");
        }
    }
}
