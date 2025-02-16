// See https://aka.ms/new-console-template for more information
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class Program
{

    static void Main(string[] args)
    {
        using (var db = new AppDbContext())
        {
            var produtoNovo = new Produto();
            produtoNovo.Preco = 22.9M;
            produtoNovo.Estoque = 10;
            produtoNovo.Nome = "Caneta Tata";

            //para incluir somente um objeto no banco atráves do  ef
            db.Add(produtoNovo);
            db.SaveChanges();

            //para incluir uma lista de objetos atraves do ef
            var listaProdutos = new List<Produto>
            {
                new Produto { Nome="Teste1", Preco=11.22M, Estoque=1 },
                new Produto { Nome="Teste2", Preco=11.22M, Estoque=1 },

            };
            db.AddRange(listaProdutos);
            db.SaveChanges();


            //populando produtos via código (mesma coisa do de cima, porém com um método separado)
            SeedDataBase.SeedProdutos(db);

            //como deletar dados
            var produtoASerDeletado = db.Produtos.First();
            db.Produtos.Remove(produtoASerDeletado);
            db.SaveChanges();
            //Depois de qualquer alteração, é necessário utilizar o db.SaveChanges();

            //Atualizar/Editar os produtos
            var produtoASerAtualizado = db.Produtos.Where(p => p.IdProduto == 24).FirstOrDefault();
            produtoASerAtualizado.Nome = "Caneta Fofa";
            produtoASerAtualizado.Preco = 33.2M;
            db.SaveChanges();

            //Exibindo produtos no console
            ExibirProdutos(db);
        }
    }
    private static void ExibirProdutos(AppDbContext db)
    {
        //exibe produtos
        var produtos = db.Produtos.ToList();
        foreach (var produto in produtos)
        {
            Console.WriteLine(produto.Nome);
        }
        Console.ReadLine();
    }
}
//entidades
public class Produto
{
    [Key]
    public int IdProduto { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}

public class AppDbContext : DbContext
{
    //mapeamento da entidade para a tabela
    public DbSet<Produto> Produtos { get; set; }

    //o provedor e a string de conexão
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-ILHUQ20;Initial Catalog=teste;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}