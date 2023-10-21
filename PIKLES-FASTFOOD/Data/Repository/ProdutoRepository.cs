using Data.Context;
using Domain.Base;
using Domain.Entities;
using Domain.Port.DrivenPort;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly MySQLContext _context;

        public ProdutoRepository(MySQLContext context)
        {
            _context = context;
        }

        public async Task<List<Produto>> GetProdutos()
        {
            return await _context.Produto.ToListAsync();
        }

        public async Task<Produto> GetProdutoById(int id)
        {
            return await _context.Produto.FindAsync(id);
        }

        public async Task<Produto> PostProduto(Produto produto)
        {
            _context.Produto.Add(produto);
            _context.SaveChanges();

            return produto;
        }

        public async Task<int> UpdateProduto(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> PutProduto(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteProduto(int id)
        {
           var produto = await _context.Produto.FindAsync(id);

           if (produto != null)
           {
               _context.Produto.Remove(produto);
           }    

            return await _context.SaveChangesAsync();
        }

        public async Task<List<Produto>> GetProdutosByIdCategoria(EnumCategoria? idCategoria)
        {
            List<Produto> produtos = new();

            if (_context.Produto is not null)
            {
                produtos = await _context.Produto.Where(x => x.IdCategoriaProduto == idCategoria).ToListAsync();
            }

            return produtos;
        }
    }
}
