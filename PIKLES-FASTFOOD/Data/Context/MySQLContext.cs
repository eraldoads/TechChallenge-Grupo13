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
                // Configurações do DbContext.
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração das entidades do modelo, incluindo chaves primárias, chaves estrangeiras e outros relacionamentos.
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<Produto>().HasKey(p => p.IdProduto);
            modelBuilder.Entity<ProdutoLista>().HasKey(p => p.IdProduto);
            modelBuilder.Entity<Categoria>().HasKey(c => c.IdCategoria);
            modelBuilder.Entity<Pedido>().HasKey(p => p.IdPedido);
            modelBuilder.Entity<Combo>().HasKey(c => c.IdCombo);
            modelBuilder.Entity<ComboProduto>().HasKey(pc => pc.IdProdutoCombo);

            modelBuilder.Entity<Produto>()
                        .HasOne(p => p.Categoria)
                        .WithMany(c => c.Produtos)
                        .HasForeignKey(p => p.IdCategoria);

            modelBuilder.Entity<Produto>()
                        .Navigation(p => p.Categoria)
                        .UsePropertyAccessMode(PropertyAccessMode.Property);

            // Mais configurações e relacionamentos podem ser definidos aqui.
        }

        public DbSet<Cliente>? Cliente { get; set; }
        public DbSet<Produto>? Produto { get; set; }
        public DbSet<ProdutoLista>? ProdutoLista { get; set; }
        public DbSet<Categoria>? Categoria { get; set; }
        public DbSet<Pedido>? Pedido { get; set; }
        public DbSet<Combo>? Combo { get; set; }
        public DbSet<ComboProduto>? ComboProduto { get; set; }
    }
}
