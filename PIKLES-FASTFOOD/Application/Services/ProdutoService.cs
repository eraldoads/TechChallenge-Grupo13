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

        /// <summary>
        /// Construtor para a classe ProdutoService.
        /// </summary>
        /// <param name="produtoRepository">O repositório de produtos a ser usado pela classe ProdutoService.</param>
        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        /// <summary>
        /// Obtém um produto pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do produto.</param>
        /// <returns>Retorna o produto se encontrado; caso contrário, retorna null.</returns>
        public async Task<Produto?> GetProdutoById(int id)
        {
            var produto = await _produtoRepository.GetProdutoById(id);

            return produto;
        }

        /// <summary>
        /// Obtém todos os produtos.
        /// </summary>
        /// <returns>Retorna uma lista de produtos.</returns>
        public async Task<List<Produto>> GetProdutos()
        {
            return await _produtoRepository.GetProdutos();
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="produtoDTO">O DTO do produto a ser criado.</param>
        /// <returns>Retorna o produto criado.</returns>
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

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="idProduto">O ID do produto a ser atualizado.</param>
        /// <param name="produtoInput">O produto com as informações atualizadas.</param>
        public async Task PutProduto(int idProduto, Produto produtoInput)
        {
            var produto = await _produtoRepository.GetProdutoById(idProduto) ?? throw new ResourceNotFoundException("Produto não encontrado.");

            produto.NomeProduto = produtoInput.NomeProduto;
            produto.DescricaoProduto = produtoInput.DescricaoProduto;
            produto.ValorProduto = produtoInput.ValorProduto;
            produto.IdCategoria = produto.IdCategoria;

            await UpdateProduto(produto);
        }

        /// <summary>
        /// Atualiza parcialmente um produto existente.
        /// </summary>
        /// <param name="idProduto">O ID do produto a ser atualizado.</param>
        /// <param name="patchDoc">O JsonPatchDocument com as operações de atualização.</param>
        public async Task PatchProduto(int idProduto, JsonPatchDocument<Produto> patchDoc)
        {
            var produto = await _produtoRepository.GetProdutoById(idProduto) ?? throw new ResourceNotFoundException("Produto não encontrado.");
            patchDoc.ApplyTo(produto);

            await UpdateProduto(produto);
        }

        /// <summary>
        /// Exclui um produto existente.
        /// </summary>
        /// <param name="id">O ID do produto a ser excluído.</param>
        /// <returns>Retorna o produto excluído.</returns>
        public async Task<Produto> DeleteProduto(int id)
        {
            try
            {
                var produto = await GetProdutoById(id) ?? throw new KeyNotFoundException("Produto não encontrado.");
                await _produtoRepository.DeleteProduto(id);

                return produto;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtém todos os produtos por categoria.
        /// </summary>
        /// <param name="idCategoria">A categoria dos produtos a serem obtidos.</param>
        /// <returns>Retorna uma lista de produtos da categoria especificada.</returns>
        public async Task<List<Categoria>> GetProdutosByIdCategoria(EnumCategoria? idCategoria)
        {
            return await _produtoRepository.GetProdutosByIdCategoria(idCategoria);
        }


        // Métodos Privados.

        #region [Métodos Privados]
        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="produto">O produto com as informações atualizadas.</param>
        private async Task UpdateProduto(Produto produto)
        {
            if (!produto.IsValid())
                throw new ValidationException("Dados inválidos.");

            await _produtoRepository.UpdateProduto(produto);
        }

        /// <summary>
        /// Valida um ProdutoDTO.
        /// </summary>
        /// <param name="produtoDTO">O ProdutoDTO a ser validado.</param>
        /// <param name="results">A coleção de resultados de validação.</param>
        /// <returns>Retorna verdadeiro se o ProdutoDTO for válido; caso contrário, retorna falso.</returns>
        private static bool ValidateProdutoDTO(ProdutoDTO produtoDTO, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(produtoDTO, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(produtoDTO, context, results, true);
        }

        /// <summary>
        /// Mapeia um ProdutoDTO para um Produto.
        /// </summary>
        /// <param name="produtoDto">O ProdutoDTO a ser mapeado.</param>
        /// <returns>Retorna um Produto que corresponde ao ProdutoDTO.</returns>
        private static Produto MapProdutoDtoToProduto(ProdutoDTO produtoDto)
        {
            return new Produto
            {
                NomeProduto = produtoDto.NomeProduto,
                DescricaoProduto = produtoDto.DescricaoProduto,
                ValorProduto = produtoDto.ValorProduto,
                IdCategoria = produtoDto.IdCategoria
            };
        }
        #endregion
    }
}
