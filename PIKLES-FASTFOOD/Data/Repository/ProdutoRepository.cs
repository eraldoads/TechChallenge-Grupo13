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

        /// <summary>
        /// Inicializa uma nova instância do repositório de produtos com o contexto fornecido.
        /// </summary>
        /// <param name="context">O contexto MySQL para o repositório de produtos.</param>
        public ProdutoRepository(MySQLContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os produtos do contexto do banco de dados.
        /// </summary>
        /// <returns>Uma lista de todos os produtos no contexto.</returns>
        public async Task<List<Produto>> GetProdutos()
        {
            if (_context.Produto is not null)
                return await _context.Produto.ToListAsync();

            return new List<Produto>();
        }

        /// <summary>
        /// Obtém um produto pelo ID no contexto do banco de dados.
        /// </summary>
        /// <param name="id">O ID do produto a ser recuperado.</param>
        /// <returns>O produto correspondente ao ID fornecido.</returns>
        public async Task<Produto?> GetProdutoById(int id)
        {
            if (_context.Produto is null)
                throw new InvalidOperationException("Contexto de produto nulo.");

            var produto = await _context.Produto.FindAsync(id);

            return produto;
        }

        /// <summary>
        /// Adiciona um novo produto ao contexto do banco de dados.
        /// </summary>
        /// <param name="produto">O produto a ser adicionado ao contexto.</param>
        /// <returns>O produto recém-adicionado.</returns>
        public async Task<Produto> PostProduto(Produto produto)
        {
            if (_context.Produto is not null)
            {
                _context.Produto.Add(produto);
                await _context.SaveChangesAsync();
            }

            return produto;
        }

        /// <summary>
        /// Atualiza um produto existente no contexto do banco de dados.
        /// </summary>
        /// <param name="produto">O produto a ser atualizado no contexto.</param>
        /// <returns>O número de entradas modificadas no contexto do banco de dados.</returns>
        public async Task<int> UpdateProduto(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Substitui um produto existente no contexto do banco de dados.
        /// </summary>
        /// <param name="produto">O produto a ser substituído no contexto.</param>
        /// <returns>O número de entradas modificadas no contexto do banco de dados.</returns>
        public async Task<int> PutProduto(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Exclui um produto com o ID fornecido do contexto do banco de dados.
        /// </summary>
        /// <param name="id">O ID do produto a ser excluído.</param>
        /// <returns>O número de entradas modificadas no contexto do banco de dados.</returns>
        public async Task<int> DeleteProduto(int id)
        {
            if (_context.Produto is null)
                throw new InvalidOperationException("O contexto do produto é nulo.");

            var produto = await _context.Produto.FindAsync(id) ?? throw new KeyNotFoundException($"O produto com o ID {id} não foi encontrado.");

            _context.Produto.Remove(produto);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtém todos os produtos de uma determinada categoria do contexto do banco de dados.
        /// </summary>
        /// <param name="idCategoria">O ID da categoria de produtos.</param>
        /// <returns>Uma lista de produtos correspondentes à categoria fornecida.</returns>
        public async Task<List<Produto>> GetProdutosByIdCategoria(EnumCategoria? idCategoria)
        {
            List<Produto> produtos = new();

            if (_context.Produto is not null)
                produtos = await _context.Produto.Where(x => x.IdCategoriaProduto == idCategoria).ToListAsync();

            return produtos;
        }
    }
}
