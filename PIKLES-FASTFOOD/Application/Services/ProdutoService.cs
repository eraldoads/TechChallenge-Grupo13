using Domain.Base;
using Domain.Entities;
using Domain.EntitiesDTO;
using Domain.Port.DrivenPort;
using Domain.Port.Services;
using System.ComponentModel.DataAnnotations;

namespace Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Produto> GetProdutoById(int? id)
        {
            return await _produtoRepository.GetProdutoById(id.Value);
        }

        public async Task<List<Produto>> GetProdutos()
        {
            return await _produtoRepository.GetProdutos();
        }

        public async Task<Produto> PostProduto(ProdutoDTO produtoDTO)
        {
            if (!ValidateProdutoDTO(produtoDTO, out ICollection<ValidationResult> results))
            {
                var errors = results.Select(r => r.ErrorMessage).ToList();
                throw new ValidationException("Ocorreu um erro de validação", new Exception(string.Join("\n", errors)));
            }

            var produto = MapProdutoDtoToProduto(produtoDTO);
            return await _produtoRepository.PostProduto(produto);
        }

        public async Task UpdateProduto(Produto produto)
        {
            if (produto == null)
                throw new ValidationException("Produto não pode ser nulo.");

            var itemProduto = await _produtoRepository.GetProdutoById(produto.IdProduto);

            if (itemProduto == null)
            {
                throw new ValidationException("Produto não encontrado.");
            }
            else
            {
                itemProduto = produto;
            }

            await _produtoRepository.UpdateProduto(itemProduto);
        }

        public async Task<int> DeleteProduto(int id)
        {
            return await _produtoRepository.DeleteProduto(id);
        }

        private static bool ValidateProdutoDTO(ProdutoDTO produtoDTO, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(produtoDTO, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(produtoDTO, context, results, true);
        }

        private static Produto MapProdutoDtoToProduto(ProdutoDTO produtoDto)
        {
            return new Produto
            {
                NomeProduto = produtoDto.NomeProduto,
                DescricaoProduto = produtoDto.DescricaoProduto,
                ValorProduto = produtoDto.ValorProduto,
                IdCategoriaProduto = produtoDto.IdCategoriaProduto
            };            
        }

        public async Task<List<Produto>> GetProdutosByIdCategoria(EnumCategoria? idCategoria)
        {
            return await _produtoRepository.GetProdutosByIdCategoria(idCategoria);
        }
    }
}
