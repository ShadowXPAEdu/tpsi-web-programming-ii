using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity;

namespace TrabalhoPartico.Models
{
    public class ProdQuant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Produto Produto { get; set; }
        [Required]
        public int Quantidade { get; set; }

        private ProdQuant() { }

        public ProdQuant(Produto prod, int quant)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            this.Id = db.Ids.Find("ProdQuant").IdNum++;
            this.Produto = db.Produtos.Find(prod.Id);
            this.Produto.Categoria = db.Categorias.SqlQuery("SELECT [Cat1].[Id] AS[Id], [Cat1].[Nome] AS[Nome], [Cat1].[Descricao] AS[Descricao]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Categorias] AS[Cat1] WHERE[Pro1].[Categoria_Id] = [Cat1].[Id] and [Pro1].[Id] = @idProd", new SqlParameter("@idProd", prod.Id)).FirstOrDefault();
            db.Entry(this.Produto).State = EntityState.Unchanged;
            db.Entry(this.Produto.Categoria).State = EntityState.Unchanged;
            db.SaveChanges();
            this.Quantidade = quant;
        }
    }
}