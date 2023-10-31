using Newtonsoft.Json;

namespace Domain.Entities
{
    public class Combo
    {
        [JsonProperty("idCombo")]
        public int IdCombo { get; set; }

        [JsonProperty("pedidoId")]
        public int PedidoId { get; set; }

        // Propriedade para armazenar os produtos relacionados ao combo
        public List<ComboProduto> Produtos { get; set; }

        // Adicionar um construtor para inicializar a lista de produtos
        public Combo()
        {
            Produtos = new List<ComboProduto>();
        }
    }
}
