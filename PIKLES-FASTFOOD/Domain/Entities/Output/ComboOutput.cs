using System.Text.Json.Serialization;

namespace Domain.Entities.Output
{
    public class ComboOutput
    {
        [JsonIgnore]
        public int IdCombo { get; set; }
        public List<ProdutoOutput>? Produto { get; set; }
    }
}
