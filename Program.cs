// See https://aka.ms/new-console-template for more information
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using(var db = new AppDbContext())
{
    //populando produtos via código
    SeedDataBase.SeedProdutos(db);

    //exibe produtos
    var produtos = db.Produtos.ToList();
    foreach (var produto in produtos)
    {
        Console.WriteLine(produto.Nome);
    }
    Console.ReadLine();
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