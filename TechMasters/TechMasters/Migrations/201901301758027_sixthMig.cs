namespace TrabalhoPartico.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sixthMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendas", "DataCompra", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendas", "DataCompra");
        }
    }
}
