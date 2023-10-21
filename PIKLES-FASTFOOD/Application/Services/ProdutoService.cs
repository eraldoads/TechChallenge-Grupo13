using Domain.Base;
using Domain.Entities;
using Domain.EntitiesDTO;
using Domain.Port.DrivenPort;
using Domain.Port.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
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

        public async Task PutProduto(int idProduto, Produto produtoInput)
        {
            var produto = await _produtoRepository.GetProdutoById(idProduto) ?? throw new ResourceNotFoundException("Produto não encontrado.");

            produto.NomeProduto = produtoInput.NomeProduto;
            produto.DescricaoProduto = produtoInput.DescricaoProduto;
            produto.ValorProduto = produtoInput.ValorProduto;
            produto.IdCategoriaProduto = produto.IdCategoriaProduto;

            await UpdateProduto(produto);
        }

        public async Task PatchProduto(int idProduto, JsonPatchDocument<Produto> patchDoc)
        {
            var produto = await _produtoRepository.GetProdutoById(idProduto) ?? throw new ResourceNotFoundException("Produto não encontrado.");
            patchDoc.ApplyTo(produto);

            await UpdateProduto(produto);
        }

        public async Task<int> DeleteProduto(int id)
        {
            return await _produtoRepository.DeleteProduto(id);
        }

        private async Task UpdateProduto(Produto produto)
        {
            if (!produto.IsValid())
                throw new ValidationException("Dados inválidos.");

            await _produtoRepository.UpdateProduto(produto);
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
