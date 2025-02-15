using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework
{
    public class SeedDataBase
    {
        public static void SeedProdutos(AppDbContext context)
        {
            if (context.Produtos.Any())
            {
                var produtos = new List<Produto>()
                {
                    new Produto { Nome = "Caneta Chimmy", Preco = 4.99M, Estoque = 10 },
                    new Produto { Nome = "Caneta Chimmy", Preco = 4.99M, Estoque = 10 }
                };

                context.AddRange(produtos);
                context.SaveChanges();
            }
        }
    }
}
