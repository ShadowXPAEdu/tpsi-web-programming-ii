using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TrabalhoPartico.Models
{
    public class Encomendas
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public List<ProdQuant> Produtos { get; set; }
        [Required]
        public double PrecoTotal { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public DateTime DataEncomenda { get; set; }

        public Encomendas() { }
        public Encomendas(List<ProdQuant> prods, double preco)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            this.Id = db.Ids.Find("Encomenda").IdNum++;
            db.SaveChanges();
            this.Produtos = prods;
            this.PrecoTotal = preco;
            this.Estado = "Pendente";
            this.DataEncomenda = DateTime.Now;
        }

        /*
        public void AddProduto(Produto produto, int quant)
        {
            if (quant > 0)
            {
                if (!Produtos.Contains(produto))
                {
                    Produtos.Add(produto);
                    quantidade.Add(quant);
                }
                else
                {
                    int index = Produtos.IndexOf(produto);
                    quantidade.Insert(index, quantidade[index] + quant);
                }
            }
        }

        public void RemoverProduto(Produto produto)
        {
            int index = Produtos.IndexOf(produto);
            quantidade.Remove(quantidade[index]);
            Produtos.Remove(produto);
        }

        public void RemoverProduto(Produto produto, int quant)
        {
            if (quant > 0)
            {
                int index = Produtos.IndexOf(produto);
                int oldQuant = quantidade[index];
                if (oldQuant - quant > 0)
                {
                    quantidade.Insert(index, oldQuant - quant);
                }
                else
                {
                    RemoverProduto(produto);
                }
            }
        }
        */
    }
}