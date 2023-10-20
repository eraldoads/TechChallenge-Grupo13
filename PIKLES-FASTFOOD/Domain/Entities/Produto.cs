using Domain.Base;
using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [SwaggerSchemaFilter(typeof(ProdutoSchemaFilter))]
    public class Produto
    {
        [JsonProperty("idProduto")]
        public int IdProduto { get; set; }

        [JsonProperty("nomeProduto")]
        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres")]
        public string? NomeProduto { get; set; }

        [JsonProperty("valorProduto")]
        [Required(ErrorMessage = "O valor do produto é obrigatório")]
        [Range(0.01, float.MaxValue, ErrorMessage = "O valor do produto deve ser um número positivo maior que zero")]
        public float ValorProduto { get; set; }

        [JsonProperty("idCategoriaProduto")]
        [Required(ErrorMessage = "O IdCategoria do produto é obrigatória")]
        public EnumCategoria IdCategoriaProduto { get; set; }

        [JsonProperty("descricaoProduto")]
        [Required(ErrorMessage = "A descrição do produto é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição do produto deve ter no máximo 500 caracteres")]
        public string? DescricaoProduto { get; set; }


    }
}
