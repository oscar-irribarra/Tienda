namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class barcodeisunique : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Producto", "Barcode", unique: true, name: "Producto_Barcode_Index");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Producto", "Producto_Barcode_Index");
        }
    }
}
