using System.ComponentModel.DataAnnotations;

namespace TrabalhoPartico.Models
{
    public class ids
    {
        [Key]
        public string Nome { get; set; }
        [Required]
        public int IdNum { get; set; }

        private ids()
        { }

        public ids(string nome)
        {
            this.Nome = nome;
            this.IdNum = 0;
        }
    }
}