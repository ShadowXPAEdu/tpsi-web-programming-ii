namespace TrabalhoPartico.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secMig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProdQuants", "Carrinho_Id", "dbo.Carrinhoes");
            DropIndex("dbo.ProdQuants", new[] { "Carrinho_Id" });
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Morada = c.String(nullable: false),
                        NIF = c.String(nullable: false),
                        Telefone = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vendas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cliente_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.Cliente_Id, cascadeDelete: true)
                .Index(t => t.Cliente_Id);
            
            AddColumn("dbo.ProdQuants", "Venda_Id", c => c.Int());
            AddColumn("dbo.Encomendas", "Estado", c => c.String(nullable: false));
            AlterColumn("dbo.Encomendas", "PrecoTotal", c => c.Double(nullable: false));
            CreateIndex("dbo.ProdQuants", "Venda_Id");
            AddForeignKey("dbo.ProdQuants", "Venda_Id", "dbo.Vendas", "Id");
            DropColumn("dbo.ProdQuants", "Carrinho_Id");
            DropTable("dbo.Carrinhoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Carrinhoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ProdQuants", "Carrinho_Id", c => c.Int());
            DropForeignKey("dbo.ProdQuants", "Venda_Id", "dbo.Vendas");
            DropForeignKey("dbo.Vendas", "Cliente_Id", "dbo.Clientes");
            DropIndex("dbo.Vendas", new[] { "Cliente_Id" });
            DropIndex("dbo.ProdQuants", new[] { "Venda_Id" });
            AlterColumn("dbo.Encomendas", "PrecoTotal", c => c.Single(nullable: false));
            DropColumn("dbo.Encomendas", "Estado");
            DropColumn("dbo.ProdQuants", "Venda_Id");
            DropTable("dbo.Vendas");
            DropTable("dbo.Clientes");
            CreateIndex("dbo.ProdQuants", "Carrinho_Id");
            AddForeignKey("dbo.ProdQuants", "Carrinho_Id", "dbo.Carrinhoes", "Id");
        }
    }
}
