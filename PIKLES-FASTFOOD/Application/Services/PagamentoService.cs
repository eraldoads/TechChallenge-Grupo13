using Domain.Entities;
using Domain.Entities.Output;
using Domain.Interfaces;
using Domain.ValueObjects;
using Newtonsoft.Json;

namespace Application.Interfaces
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public PagamentoService(IPagamentoRepository pagamentoRepository, IPedidoRepository pedidoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PagamentoStatusOutput?> GetStatusPagamento(int idPedido)
        {
            // Implemente a lógica para obter o status do pagamento com base no idPedido.
            var pagamentoStatus = await _pagamentoRepository.GetPagamentoByIdPedido(idPedido);

            if (pagamentoStatus == null)
                return null;

            return new PagamentoStatusOutput
            {
                StatusPagamento = pagamentoStatus.StatusPagamento,
            };
        }

        public async Task<PagamentoOutput> ProcessarPagamento(Pagamento pagamentoInput)
        {
            // Carregar o Pedido correspondente
            var pedido = await _pedidoRepository.GetPedidoById(pagamentoInput.IdPedido) ??
                throw new PreconditionFailedException($"Pedido com ID {pagamentoInput.IdPedido} não existe. Operação cancelada", nameof(pagamentoInput.IdPedido));

            // Verifica se o status do pedido. Deve estar como "Recebido", ou seja, quando o cliente selecionou os itens/combo
            // e fechou o carrinho, o pedido foi criado com o status de recebido e o endpoint "ProcessarPagamento" deve ser chamado.
            if (pedido.StatusPedido is not null && pedido.StatusPedido.ToUpper() != "RECEBIDO")
                throw new PreconditionFailedException($"Pedido com ID {pagamentoInput.IdPedido} não pode ser processado o pagamento, pois seu status atual está como '{pedido.StatusPedido}'. Operação cancelada", nameof(pagamentoInput.IdPedido));

            // TODO: Chama o endpoint de PATCH /pedido/{idPedido} e atualiza o status se aprovado para "Em Preparação", caso o pagamento não seja aprovado,
            // permanece como "Recebido". - Aguardar o WebHook ficar pronto, pois essa logica pode ser chamado por ele e aqui seja chamado, o
            // endpointo do WebHook.


            // Associar o pagamento ao pedido
            pagamentoInput.Pedido = pedido;

            // Salvar o pagamento
            var novoPagamento = await _pagamentoRepository.PostPagamento(pagamentoInput);

            // Mapear para PagamentoOutput antes de retornar
            var pagamentoOutput = new PagamentoOutput
            {
                IdPagamento = novoPagamento.IdPagamento,
                ValorPagamento = novoPagamento.ValorPagamento,
                MetodoPagamento = novoPagamento.MetodoPagamento,
                StatusPagamento = novoPagamento.StatusPagamento,
                IdPedido = novoPagamento.IdPedido
            };

            return pagamentoOutput;
        }

        /*
          WEBHOOK
         
         // TODO: quando o retorno do WEBHOOK for igual a '"action": "payment.created", ' deverá chamar o endpointo de PATCH Pedidos 
         // e atualizar o status do pedido para "Em Preparação".
         
         */

        public async Task<QRCodeOutput?> CriarQRCodePagamento(int idPedido)
        {
            // Implemente a lógica para obter o status do pagamento com base no idPedido.
            var pagamento = await _pagamentoRepository.GetPagamentoByIdPedido(idPedido);

            if (pagamento == null)
                return null;

            double valorPagamento = (double)Math.Round(pagamento.ValorPagamento, 2);

            var payLoad =  new PayloadQRCodeOutput()
            {
                description = "Pagamento",
                external_reference = pagamento.IdPagamento.ToString(),
                items = new List<Item>()
                {
                    new Item()
                    {
                        title = "Pagamento",
                        description = "Pagamento do Pedido",
                        unit_price = valorPagamento,
                        quantity = 1,                        
                        unit_measure = "unit",                        
                        total_amount = valorPagamento,
                    }
                },
                notification_url = "https://webhook.site/0ed53b32-a18f-4d3f-acbf-6cb022187561",
                sponsor = new Sponsor()
                {
                    id = 58923039
                },
                title = "Pagamento do Pedido",
                total_amount = valorPagamento
            };

            var qrCode = await CriarOrdemMercadoPago(payLoad);
            return qrCode;
        }

        private async Task<QRCodeOutput> CriarOrdemMercadoPago(PayloadQRCodeOutput payLoad)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, "https://api.mercadopago.com/instore/orders/qr/seller/collectors/1619908702/pos/SUC001POS001/qrs");
            request.Headers.Add("Authorization", "Bearer TEST-1380797655451164-010212-2bb955330b75683baa093d93183e0c2e-1619908702");
            var json = JsonConvert.SerializeObject(payLoad);
            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();
            var qrcodeOutput = JsonConvert.DeserializeObject<QRCodeOutput>(responseJson);
            return qrcodeOutput;
        }

    }
}
