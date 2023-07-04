namespace TrabalhoPartico.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fifthMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produtoes", "Desconto", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produtoes", "Desconto");
        }
    }
}
