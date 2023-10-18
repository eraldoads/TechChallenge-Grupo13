using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Aqui você pode configurar as opções do DbContext, como a string de conexão, o provedor do banco de dados, etc.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aqui você pode configurar as entidades do modelo, como as chaves primárias, os relacionamentos, os índices, etc.

            // Configura a chave primária composta
            modelBuilder.Entity<Pedido_Produto>().HasKey(pp => new { pp.IdPedido, pp.IdProduto });
            // Configura a chave primária da entidade Pedido
            modelBuilder.Entity<Pedido>().HasKey(p => p.Id);
            // Configura a propriedade IdPedido como uma coluna que gera um valor sequencial para cada registro inserido
            modelBuilder.Entity<Pedido_Produto>().Property(p => p.IdPedido).ValueGeneratedOnAdd();
            // Configura uma chave alternativa para a entidade Pedido_Produto usando as propriedades IdPedido e IdProduto
            // Isso permite que existam mais de um pedido_produto com o mesmo IdPedido, desde que o IdProduto seja diferente
            modelBuilder.Entity<Pedido_Produto>().HasAlternateKey(p => new { p.IdPedido, p.IdProduto });

        }

        public DbSet<Cliente>? Cliente { get; set; }
        public DbSet<Produto>? Produto { get; set; }
        public DbSet<Categoria>? Categoria { get; set; }
        public DbSet<Pedido>? Pedido { get; set; }
        public DbSet<Pedido_Produto>? Pedido_Produto { get; set; }
        //public DbSet<Combo>? Combo { get; set; }

    }
}
