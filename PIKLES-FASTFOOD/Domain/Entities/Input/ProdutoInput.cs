using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Input
{
    public class ProdutoInput
    {
        [Required(ErrorMessage = "O id do produto é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O id do produto deve ser um número positivo")]
        public int IdProduto { get; set; } // O id do produto

        [Required(ErrorMessage = "A quantidade do produto é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade do produto deve ser um número positivo")]
        public int Quantidade { get; set; } // A quantidade do produto
    }

}
