using Newtonsoft.Json;

namespace App.Application.ViewModels.Response
{
    public class Cliente
    {
        [JsonProperty("nome")]
        public string? Nome { get; set; }

        [JsonProperty("cpf")]
        public string? CPF { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
