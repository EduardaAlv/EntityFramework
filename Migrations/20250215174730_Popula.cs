using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
//Popular o banco atráveis de migrations
namespace EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Popula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Produtos(Nome, Preco, Estoque) Values ('Caneta', 4.99, 10)");
            mb.Sql("INSERT INTO Produtos(Nome, Preco, Estoque) Values ('Caneta Morango', 14.99, 10)");
            mb.Sql("INSERT INTO Produtos(Nome, Preco, Estoque) Values ('Caneta Koya', 14.99, 10)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("delete from produtos");

        }
    }
}
