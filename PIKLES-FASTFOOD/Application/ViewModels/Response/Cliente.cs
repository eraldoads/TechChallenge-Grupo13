using Newtonsoft.Json;

namespace App.Application.ViewModels.Response
{
    public class Cliente
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string? Nome { get; set; }

        [JsonProperty("Sobrenome")]
        public string? Sobrenome { get; set; }

        [JsonProperty("cpf")]
        public string? CPF { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
