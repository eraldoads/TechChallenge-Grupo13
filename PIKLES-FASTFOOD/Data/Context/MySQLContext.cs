using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Aqui você pode configurar as opções do DbContext, como a string de conexão, o provedor do banco de dados, etc.
            if (!optionsBuilder.IsConfigured)
            {
                // Configurações do DbContext
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aqui você pode configurar as entidades do modelo, como as chaves primárias, os relacionamentos, os índices, etc.
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<Produto>().HasKey(p => p.IdProduto);
            modelBuilder.Entity<Categoria>().HasKey(c => c.IdCategoria);
            modelBuilder.Entity<Pedido>().HasKey(p => p.IdPedido);
            modelBuilder.Entity<Combo>().HasKey(c => c.IdCombo);
            modelBuilder.Entity<ComboProduto>().HasKey(pc => pc.IdProdutoCombo);

            // Defina relacionamentos e outras configurações aqui
        }

        public DbSet<Cliente>? Cliente { get; set; }
        public DbSet<Produto>? Produto { get; set; }
        public DbSet<Categoria>? Categoria { get; set; }
        public DbSet<Pedido>? Pedido { get; set; }
        public DbSet<Combo>? Combo { get; set; }
        public DbSet<ComboProduto>? ComboProduto { get; set; }
    }
}
