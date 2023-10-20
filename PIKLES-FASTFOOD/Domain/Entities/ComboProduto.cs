using Newtonsoft.Json;

namespace Domain.Entities
{
    public class ComboProduto
    {
        [JsonProperty("idProdutoCombo")]
        public int IdProdutoCombo { get; set; }

        [JsonProperty("idProduto")]
        public int IdProduto { get; set; }

        [JsonProperty("comboId")]
        public int ComboId { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }
    }
}
