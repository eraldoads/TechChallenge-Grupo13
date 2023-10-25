using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Input
{
    public class ComboInput
    {
        [Required(ErrorMessage = "A lista de produtos é obrigatória")]
        [MinLength(1, ErrorMessage = "A lista de produtos deve ter pelo menos um item")]
        public List<ProdutoInput>? Produto { get; set; } = new List<ProdutoInput>();
    }
}
