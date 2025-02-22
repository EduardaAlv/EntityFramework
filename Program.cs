// See https://aka.ms/new-console-template for more information
using EntityFramework;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

public class Program
{
    static void Main(string[] args)
    {
        using (var db = new AppDbContext())
        {
            //var produtoNovo = new Produto();
            //produtoNovo.Preco = 22.9M;
            //produtoNovo.Estoque = 10;
            //produtoNovo.Nome = "Caneta Tata";

            ////para incluir somente um objeto no banco atráves do  ef
            //db.Add(produtoNovo);
            //db.SaveChanges();

            ////para incluir uma lista de objetos atraves do ef
            //var listaProdutos = new List<Produto>
            //{
            //    new Produto { Nome="Teste1", Preco=11.22M, Estoque=1 },
            //    new Produto { Nome="Teste2", Preco=11.22M, Estoque=1 },

            //};
            //db.AddRange(listaProdutos);
            //db.SaveChanges();


            ////populando produtos via código (mesma coisa do de cima, porém com um método separado)
            //SeedDataBase.SeedProdutos(db);

            ////como deletar dados
            //var produtoASerDeletado = db.Produtos.First();
            //db.Produtos.Remove(produtoASerDeletado);
            //db.SaveChanges();
            ////Depois de qualquer alteração, é necessário utilizar o db.SaveChanges();

            ////Atualizar/Editar os produtos
            //var produtoASerAtualizado = db.Produtos.Where(p => p.IdProduto == 24).FirstOrDefault();
            //produtoASerAtualizado.Nome = "Caneta Fofa";
            //produtoASerAtualizado.Preco = 33.2M;
            //db.SaveChanges();


            ////Tem como utilizar só um SaveChanges para várias opções
            ////Se algo der falha, o Rollback será realizado

            //// INCLUIR PRODUTO
            //var produto = new Produto
            //{
            //    Nome = "Pilha",
            //    Preco = 9.99M,
            //    Estoque = 99
            //};

            //db.Produtos.Add(produto);

            //// ALTERAR UM PRODUTO
            //var meuProduto = db.Produtos.Find(5);
            //meuProduto.Nome = "Nome do produto alterado";

            //// REMOVER UM PRODUTO
            //var ultimoProduto = db.Produtos.Last();
            //db.Produtos.Remove(ultimoProduto);

            ////somente 1 SaveChanges, em uma transação
            //db.SaveChanges();


            ////Exibir o estados das entidades -
            ////ChangeTracker que monitora as entidades
            ////Estados:
            ////Não modificado: Unchanged
            ////Acabou de inserir a entidade: Added
            ////Entidade foi alterada: Modified
            ////Entidade foi excluida: Deleted

            //Console.WriteLine("1- Carga Inicial");
            //var produtos = db.Produtos;
            //foreach (var p in produtos)
            //{
            //    System.Console.WriteLine(p.Nome);
            //}

            //ExibirEstado(db.ChangeTracker.Entries());

            ////Exibindo produtos no console
            //ExibirProdutos(db);


            //ExibirAutores();
            //ExibirAutoresConsultaProjecao();
            ExibirDadosExplicitingLoading();
            ExibirDadosExplicitingLoadingFiltrandoPorQuery();
        }
        Console.ReadLine();
    }

    private static void ExibirDadosExplicitingLoadingFiltrandoPorQuery()
    {
        using (var contexto = new AppDbContext())
        {
            var autor = contexto.Autores.Where(a => a.Nome == "Samuel").FirstOrDefault();
            Console.WriteLine(autor.Nome);
            //Entry é para acessar os relacionamentos de autor
            contexto.Entry(autor)
                .Collection(l => l.Livros)
                .Query().Where(l => l.AnoLancamento == 2024)
                .Load();

            foreach (var l in autor.Livros)
            {
                Console.WriteLine($"\t {l.Titulo}");
            }
        }
    }

    private static void ExibirDadosExplicitingLoading()
    {
        using (var contexto = new AppDbContext())
        {
            var autor = contexto.Autores.Where(a => a.Nome == "Samuel").FirstOrDefault();
            Console.WriteLine(autor.Nome);
            contexto.Entry(autor).Collection(l => l.Livros).Load();

            foreach (var l in autor.Livros)
            {
                Console.WriteLine($"\t {l.Titulo}");
            }
        }
    }

    private static void ExibirAutoresConsultaProjecao()
    {
        using (var contexto = new AppDbContext())
        {
            var resultado = contexto.Autores.Where(a => a.Nome == "Samuel")
                .Select(a => new
                {
                    Autor = a,
                    LivrosAutor = a.Livros
                })
                .FirstOrDefault();

            Console.WriteLine(resultado.Autor.Nome + " " + resultado.Autor.Sobrenome);
            foreach (var livro in resultado.LivrosAutor)
            {
                Console.WriteLine("\t " + livro.Titulo);
            }
        }
    }


    private static void ExibirAutores()
    {
        using (var contexto = new AppDbContext())
        {
            //é preciso pedir para carregar os relacionamentos
            //com o Include
            //Pode utilizar mais de um Include
            //E existe o ThenInclude, para pegar os relacionamentos dos relacionamentos

            //Para otimizar toda essa busca, se for muitos relacionamentos
            //Utilize o AsNoTracking(), para desativar o rastreamento dos estados das entidades
            //FAZER ISSO SOMENTE QUANDO FOR LER OS DADOS

            foreach (var autor in contexto.Autores.AsNoTracking().Include(a => a.Livros))
            {
                Console.WriteLine($"{autor.Nome}  {autor.Sobrenome}");

                foreach (var livro in autor.Livros)
                {
                    Console.WriteLine($"\t {livro.Titulo}");
                }
            }
        }

    }

    //Criar livros um por um e apontar qual o autor
    private static void IncluirAutorLivrosAddRange()
    {
        using (var contexto = new AppDbContext())
        {
            var autor = new Autor
            {
                Nome = "Stephen",
                Sobrenome = "King",
            };

            var livros = new List<Livro>
        {
            new Livro { Titulo="Carrie", AnoLancamento= 1974, Autor= autor },
            new Livro { Titulo = "A Coisa", AnoLancamento = 1986, Autor = autor },
            new Livro { Titulo = "Angústia", AnoLancamento = 1987, Autor = autor }
        };

            contexto.AddRange(livros);
            contexto.SaveChanges();
        }
    }

    //Criar um autor com varios livros
    private static void IncluirAutorELivros()
    {
        using (var contexto = new AppDbContext())
        {
            var autor = new Autor
            {
                Nome = "Samuel",
                Sobrenome = "Portes",
                Livros = new List<Livro>
            {
                new Livro{Titulo="Mercado Financeiro", AnoLancamento=2024},
                new Livro{Titulo="AI", AnoLancamento=2024}
            }

            };

            contexto.Add(autor);
            contexto.SaveChanges();
        }
    }

    //Criar um autor
    private static void IncluirAutor()
    {
        using (var contexto = new AppDbContext())
        {
            var autor = new Autor { Nome = "Samuel", Sobrenome = "Portes" };
            contexto.Add(autor);
            contexto.SaveChanges();
        }
    }

    //Inclui um Livro e referencia qual seu autor, ja existente
    private static void IncluirLivroAutor()
    {
        using (var contexto = new AppDbContext())
        {
            var autor = contexto.Autores.Find(1);

            var livro = new Livro { Titulo = "A casa torta", AnoLancamento = 1949, Autor = autor };

            contexto.Livros.Add(livro);
            contexto.SaveChanges();
        }
    }

    private static void ExibirEstado(IEnumerable<EntityEntry> entries)
    {
        foreach (var entrada in entries)
        {
            Console.WriteLine($"Estado da entidade : {entrada.State.ToString()}");
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
    public DbSet<Livro> Livros { get; set; }
    public DbSet<Autor> Autores { get; set; }

    //o provedor e a string de conexão
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-ILHUQ20;Initial Catalog=teste;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}