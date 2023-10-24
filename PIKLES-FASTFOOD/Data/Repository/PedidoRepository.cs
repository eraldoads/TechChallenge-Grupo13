using Data.Context;
using Domain.Entities;
using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.Port.DrivenPort;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly MySQLContext _context;

        public PedidoRepository(MySQLContext context)
        {
            _context = context;
        }
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
                           ORDER BY dataPedido, idCombo, nomeCompletoCliente;
                        ";

            // Execute a query SQL para obter os pedidos e suas informações relacionadas
            var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandText = query;
            _context.Database.OpenConnection();

            using (var result = command.ExecuteReader())
            {
                while (result.Read())
                {
                    var idPedido = Convert.ToInt32(result["idPedido"]);

                    if (!pedidosAgrupados.ContainsKey(idPedido))
                    {
                        var pedidoOutput = new PedidoOutput
                        {
                            IdPedido = idPedido,
                            DataPedido = Convert.ToDateTime(result["dataPedido"]),
                            StatusPedido = result["statusPedido"].ToString(),
                            NomeCliente = result["nomeCompletoCliente"].ToString(),
                            ValorTotalPedido = Convert.ToSingle(result["valorTotalPedido"]),
                            Combo = new List<ComboOutput>()
                        };

                        pedidosAgrupados[idPedido] = pedidoOutput;
                    }

                    if (pedidosAgrupados.TryGetValue(idPedido, out PedidoOutput? pedidoAgrupado))
                    {
                        if (pedidoAgrupado != null && pedidoAgrupado.Combo != null)
                        {
                            var idCombo = Convert.ToInt32(result["idCombo"]);
                            var produtoOutput = new ProdutoOutput
                            {
                                IdProduto = Convert.ToInt32(result["idProduto"]),
                                NomeProduto = result["nomeProduto"].ToString(),
                                QuantidadeProduto = Convert.ToInt32(result["quantidadeProduto"]),
                                ValorProduto = Convert.ToSingle(result["valorProduto"])
                            };

                            if (pedidoAgrupado.Combo.Any(c => c.IdCombo == idCombo))
                            {
                                var comboOutput = pedidoAgrupado.Combo.FirstOrDefault(c => c.IdCombo == idCombo);
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
            }

            _context.Database.CloseConnection();

            pedidos.AddRange(pedidosAgrupados.Values);

            return pedidos;
        }

        public async Task PostPedido(Pedido pedido)
        {         
            _context.Pedido.Add(pedido);         
            _context.SaveChangesAsync();         
        }
    }
}
