using System.ComponentModel.DataAnnotations;

namespace TrabalhoPartico.Models
{
    public class Cliente
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Morada { get; set; }
        [Required]
        public string NIF { get; set; }
        [Required]
        public string Telefone { get; set; }

        public Cliente() { }

        public Cliente(string id, string nome, string morada, string nIF, string telefone)
        {
            Id = id;
            Nome = nome;
            Morada = morada;
            NIF = nIF;
            Telefone = telefone;
        }

        public Cliente(Cliente cliente) : this(cliente.Id, cliente.Nome, cliente.Morada, cliente.NIF, cliente.Telefone)
        {}
    }
}