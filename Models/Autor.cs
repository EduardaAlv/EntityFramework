using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class Autor
    {
        [Key]
        public int AutorId { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public ICollection<Livro> Livros { get; set; }
    }
}
