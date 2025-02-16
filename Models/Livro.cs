using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class Livro
    {
        [Key]
        public int IdLivro { get; set; }
        public string Titulo { get; set; }
        public int AnoLancamento { get; set; }
        public Autor Autor { get; set; }
    }
}
