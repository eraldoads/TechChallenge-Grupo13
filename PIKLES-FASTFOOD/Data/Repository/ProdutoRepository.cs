using Data.Context;
using Domain.Base;
using Domain.Entities;
using Domain.Port.DrivenPort;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;

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
                return await _context.Produto.Include(p => p.Categoria).ToListAsync();

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

            var produto = await _context.Produto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.IdProduto == id);

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
        /// Obtém uma lista de produtos por id de categoria usando uma consulta SQL.
        /// </summary>
        /// <param name="idCategoria">O id da categoria dos produtos desejados.</param>
        /// <returns>Uma lista de objetos Produto com os dados dos produtos da categoria especificada.</returns>
        /// <exception cref="MySqlException">Se ocorrer um erro ao executar a consulta SQL.</exception>
        public async Task<List<Categoria>> GetProdutosByIdCategoria(EnumCategoria? idCategoria)
        {
            List<Categoria> categorias = new();

            if (_context.Categoria is not null)
            {
                var sqlQuery = @" SELECT PROD.IdProduto     AS IdProduto
                                , PROD.NomeProduto          AS NomeProduto
                                , PROD.ValorProduto         AS ValorProduto
                                , PROD.IdCategoria          AS IdCategoria
                                , CATE.NomeCategoria        AS NomeCategoria
                                , PROD.DescricaoProduto     AS DescricaoProduto
                                , PROD.ImagemProduto        AS ImagemProduto
                             FROM Produto PROD
                        LEFT JOIN Categoria CATE
                               ON PROD.IdCategoria = CATE.IdCategoria
                            WHERE PROD.IdCategoria = @idCategoria;";

                await using var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = sqlQuery;

                if (idCategoria.HasValue)
                {
                    command.Parameters.Add(new MySqlParameter("@idCategoria", idCategoria.Value));
                }

                await _context.Database.OpenConnectionAsync();

                await using var result = await command.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (await result.ReadAsync())
                    {
                        var categoriaId = result.GetInt32("IdCategoria");

                        var categoria = categorias.FirstOrDefault(c => c.IdCategoria == categoriaId);

                        if (categoria == null)
                        {
                            categoria = new Categoria
                            {
                                IdCategoria = categoriaId,
                                NomeCategoria = result.GetString("NomeCategoria"),
                                Produtos = new List<Produto>()
                            };
                            categorias.Add(categoria);
                        }

                        categoria.Produtos?.Add(new Produto
                        {
                            IdProduto = result.IsDBNull("IdProduto") ? 0 : result.GetInt32("IdProduto"),
                            NomeProduto = result.IsDBNull("NomeProduto") ? null : result.GetString("NomeProduto"),
                            ValorProduto = result.IsDBNull("ValorProduto") ? 0 : result.GetFloat("ValorProduto"),
                            IdCategoria = result.IsDBNull("IdCategoria") ? 0 : result.GetInt32("IdCategoria"),
                            DescricaoProduto = result.IsDBNull("DescricaoProduto") ? null : result.GetString("DescricaoProduto"),
                            ImagemProduto = result.IsDBNull("ImagemProduto") ? null : result.GetString("ImagemProduto")
                        });
                    }
                }

                _context.Database.CloseConnection();
            }

            return categorias;
        }

    }
}
