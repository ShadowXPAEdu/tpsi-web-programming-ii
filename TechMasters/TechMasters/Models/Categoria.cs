using System.ComponentModel.DataAnnotations;

namespace TrabalhoPartico.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Descricao { get; set; }

        public Categoria(string nome, string descricao)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            this.Id = db.Ids.Find("Categoria").IdNum++;
            db.SaveChanges();
            this.Nome = nome;
            this.Descricao = descricao;
        }

        public Categoria()
        {}
    }
}