namespace TrabalhoPartico.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seventhMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encomendas", "DataEncomenda", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Encomendas", "DataEncomenda");
        }
    }
}
