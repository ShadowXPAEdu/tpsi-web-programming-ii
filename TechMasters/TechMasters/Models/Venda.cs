using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrabalhoPartico.Models
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public List<ProdQuant> Produtos { get; set; }
        [Required]
        public Cliente Cliente { get; set; }
        [Required]
        public DateTime DataCompra { get; set; }

        private Venda() { }
        public Venda(List<ProdQuant> prods, Cliente cliente)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            this.Id = db.Ids.Find("Venda").IdNum++;
            db.SaveChanges();
            this.Produtos = prods;
            this.Cliente = cliente;
            this.DataCompra = DateTime.Now;
        }
    }
}