using Data.Context;
using Domain.Entities;
using Domain.Entities.Output;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly MySQLContext _context;

        /// <summary>
        /// Inicializa uma nova instância do repositório de pedidos com o contexto fornecido.
        /// </summary>
        /// <param name="context">O contexto MySQL para o repositório de pedidos.</param>
        public PedidoRepository(MySQLContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os pedidos com informações detalhadas dos clientes, combos e produtos do contexto do banco de dados.
        /// </summary>
        /// <returns>Uma lista de pedidos detalhados com informações de cliente, combo e produto.</returns>
        public async Task<List<PedidoOutput>> GetPedidos()
        {
            var pedidos = new List<PedidoOutput>();
            var pedidosAgrupados = new Dictionary<int, PedidoOutput>();

            var query = @"  SELECT  PEDI.IdPedido                           AS idPedido,
                                    PEDI.DataPedido                         AS dataPedido,
                                    PEDI.StatusPedido                       AS statusPedido,
                                    CONCAT(CLIE.Nome, ' ', CLIE.Sobrenome)  AS nomeCompletoCliente,
                                    PEDI.ValorTotal                         AS valorTotalPedido,
                                    COMB.IdCombo                            AS idCombo,
                                    PROD.IdProduto                          AS idProduto,
                                    PROD.NomeProduto                        AS nomeProduto,
                                    COMBP.Quantidade                        AS quantidadeProduto,
                                    PROD.ValorProduto                       AS valorProduto
                               FROM Pedido PEDI
                         INNER JOIN Cliente CLIE
                                 ON PEDI.IdCliente = CLIE.IdCliente
                         INNER JOIN Combo COMB
                                 ON COMB.PedidoId = PEDI.IdPedido
                         INNER JOIN ComboProduto COMBP
                                 ON COMBP.ComboId = COMB.IdCombo
                         INNER JOIN Produto PROD
                                 ON COMBP.IdProduto = PROD.IdProduto
                              WHERE PEDI.StatusPedido <> 'Finalizado'
                           ORDER BY dataPedido, idCombo, nomeCompletoCliente;
                        ";

            await using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;

            await _context.Database.OpenConnectionAsync();

            await using var result = await command.ExecuteReaderAsync();
            while (await result.ReadAsync())
            {
                var idPedido = result.GetInt32(result.GetOrdinal("idPedido"));

                if (!pedidosAgrupados.ContainsKey(idPedido))
                {
                    var pedidoOutput = new PedidoOutput
                    {
                        IdPedido = idPedido,
                        DataPedido = result.GetDateTime(result.GetOrdinal("dataPedido")),
                        StatusPedido = result.GetString(result.GetOrdinal("statusPedido")),
                        NomeCliente = result.GetString(result.GetOrdinal("nomeCompletoCliente")),
                        ValorTotalPedido = Convert.ToSingle(result["valorTotalPedido"]),
                        Combo = new List<ComboOutput>()
                    };

                    pedidosAgrupados[idPedido] = pedidoOutput;
                }

                if (pedidosAgrupados.TryGetValue(idPedido, out PedidoOutput? pedidoAgrupado))
                {
                    if (pedidoAgrupado is not null && pedidoAgrupado.Combo is not null)
                    {
                        var idCombo = result.GetInt32(result.GetOrdinal("idCombo"));
                        var produtoOutput = new ProdutoOutput
                        {
                            IdProduto = result.GetInt32(result.GetOrdinal("idProduto")),
                            NomeProduto = result.GetString(result.GetOrdinal("nomeProduto")),
                            QuantidadeProduto = result.GetInt32(result.GetOrdinal("quantidadeProduto")),
                            ValorProduto = Convert.ToSingle(result["valorProduto"])
                        };

                        if (pedidoAgrupado.Combo.Any(c => c.IdCombo == idCombo))
                        {
                            var comboOutput = pedidoAgrupado.Combo.FirstOrDefault(c => c.IdCombo == idCombo);
                            if (comboOutput?.Produto is not null)
                                comboOutput?.Produto.Add(produtoOutput);
                        }
                        else
                        {
                            var comboOutput = new ComboOutput
                            {
                                IdCombo = idCombo,
                                Produto = new List<ProdutoOutput> { produtoOutput }
                            };
                            pedidoAgrupado.Combo.Add(comboOutput);
                        }
                    }
                }
            }

            _context.Database.CloseConnection();

            pedidos.AddRange(pedidosAgrupados.Values);

            return pedidos;
        }

        /// <summary>
        /// Registra um novo pedido no contexto do banco de dados e calcula o valor total do pedido com base nos produtos e quantidades.
        /// </summary>
        /// <param name="pedido">O pedido a ser registrado.</param>
        /// <returns>O pedido registrado, incluindo o cálculo do valor total.</returns>
        public async Task<Pedido> PostPedido(Pedido pedido)
        {
            // Cálculo do valor total
            pedido.ValorTotal = CalcularValorTotal(pedido);

            if (_context.Pedido is not null)
            {
                _context.Pedido.Add(pedido);
                await _context.SaveChangesAsync();
            }

            return pedido;
        }

        /// <summary>
        /// Obtém um pedido pelo seu id no banco de dados.
        /// </summary>
        /// <param name="idPedido">O id do pedido a ser obtido.</param>
        /// <returns>Um objeto Pedido com os dados do pedido encontrado ou null se não existir.</returns>
        /// <exception cref="DbException">Se ocorrer um erro ao acessar o banco de dados.</exception>
        public async Task<Pedido?> GetPedidoById(int idPedido)
        {
            // verifica se o DbSet Pedido não é nulo.
            if (_context.Pedido is not null)
                // retorna o primeiro pedido que corresponde ao idPedido ou null se não encontrar.
                return await _context.Pedido.FirstOrDefaultAsync(p => p.IdPedido == idPedido);
            // retorna null se o DbSet Pedido for nulo.
            return null;
        }

        /// <summary>
        /// Atualiza um pedido no banco de dados.
        /// </summary>
        /// <param name="pedido">O objeto Pedido com os dados atualizados.</param>
        /// <exception cref="DbUpdateException">Se ocorrer um erro ao atualizar o banco de dados.</exception>
        public async Task UpdatePedido(Pedido pedido)
        {
            // marca o estado do pedido como modificado.
            _context.Entry(pedido).State = EntityState.Modified;
            // salva as alterações no banco de dados de forma assíncrona.
            await _context.SaveChangesAsync();
        }

        #region [Métodos de verificação]
        /// <summary>
        /// Verifica se existe um cliente com o id especificado no banco de dados.
        /// </summary>
        /// <param name="clienteId">O id do cliente a ser verificado.</param>
        /// <returns>Um valor booleano que indica se o cliente existe ou não.</returns>
        /// <exception cref="DbException">Se ocorrer um erro ao acessar o banco de dados.</exception>
        public async Task<bool> ClienteExists(int clienteId)
        {
            if (_context.Cliente is not null)
                return await _context.Cliente.AnyAsync(c => c.IdCliente == clienteId);
            return false;
        }

        /// <summary>
        /// Verifica se existe um produto com o id especificado no banco de dados.
        /// </summary>
        /// <param name="produtoId">O id do produto a ser verificado.</param>
        /// <returns>Um valor booleano que indica se o produto existe ou não.</returns>
        /// <exception cref="DbException">Se ocorrer um erro ao acessar o banco de dados.</exception>
        public async Task<bool> ProdutoExists(int produtoId)
        {
            if (_context.Produto is not null)
                return await _context.Produto.AnyAsync(p => p.IdProduto == produtoId);
            return false;
        }
        #endregion

        #region [Métodos Privados]
        /// <summary>
        /// Calcula o valor total do pedido com base nos produtos e quantidades associados a ele.
        /// "O cálculo do valor total é uma operação relacionada à persistência de dados."
        /// </summary>
        /// <param name="pedido">O pedido para o qual o valor total deve ser calculado.</param>
        /// <returns>O valor total calculado para o pedido.</returns>
        private float CalcularValorTotal(Pedido pedido)
        {
            float valorTotal = 0;

            if (_context.Produto is not null)
            {
                foreach (var combo in pedido.Combos)
                {
                    foreach (var produtoCombo in combo.Produtos)
                    {
                        var produto = _context.Produto.FirstOrDefault(p => p.IdProduto == produtoCombo.IdProduto);

                        if (produto is not null)
                        {
                            valorTotal += produto.ValorProduto * produtoCombo.Quantidade;
                        }
                    }
                }
            }

            return valorTotal;
        }
        #endregion

    }
}
