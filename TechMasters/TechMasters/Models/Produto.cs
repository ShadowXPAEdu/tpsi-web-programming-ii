using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace TrabalhoPartico.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public double Preco { get; set; }
        [Required]
        public double Desconto { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public string UrlImg { get; set; }
        [Required]
        public Categoria Categoria { get; set; }

        public static void AtualizarStock(Carrinho car)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            foreach (var pq in car.Produtos)
            {
                Produto p = db.Produtos.SingleOrDefault(pro => pro.Id == pq.Produto.Id);
                p.Id = pq.Produto.Id;
                p.Categoria = db.Categorias.SqlQuery("SELECT [Cat1].[Id] AS[Id], [Cat1].[Nome] AS[Nome], [Cat1].[Descricao] AS[Descricao]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Categorias] AS[Cat1] WHERE[Pro1].[Categoria_Id] = [Cat1].[Id] and [Pro1].[Id] = @idProd", new SqlParameter("@idProd", p.Id)).FirstOrDefault();
                p.Stock -= pq.Quantidade;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void AtualizarStockEnc(Produto prod, int quant)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            prod.Id = prod.Id;
            prod.Categoria = db.Categorias.SqlQuery("SELECT [Cat1].[Id] AS[Id], [Cat1].[Nome] AS[Nome], [Cat1].[Descricao] AS[Descricao]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Categorias] AS[Cat1] WHERE[Pro1].[Categoria_Id] = [Cat1].[Id] and [Pro1].[Id] = @idProd", new SqlParameter("@idProd", prod.Id)).FirstOrDefault();
            prod.Stock += quant;
            db.Entry(prod).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Produto(string nome, double preco, int stock, string descricao, string imgUrl, Categoria Categoria, double desconto)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            this.Id = db.Ids.Find("Produto").IdNum++;
            db.SaveChanges();
            this.Nome = nome;
            this.Preco = preco;
            this.Stock = stock;
            this.Desconto = desconto;
            this.Descricao = descricao;
            this.UrlImg = imgUrl;
            this.Categoria = Categoria;
        }

        public Produto()
        { }
    }
}