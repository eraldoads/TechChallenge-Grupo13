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
        /// Obtém todos os produtos.
        /// </summary>
        /// <returns>Retorna uma lista de produtos.</returns>
        public async Task<List<ProdutoLista>> GetProdutos()
        {
            var produtosBase = await _produtoRepository.GetProdutos();
            var produtosLista = MapParaProdutoLista(produtosBase);

            return produtosLista;
        }

        /// <summary>
        /// Obtém um produto pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do produto.</param>
        /// <returns>Retorna o produto se encontrado; caso contrário, retorna null.</returns>
        public async Task<ProdutoLista?> GetProdutoById(int id)
        {
            var produtoBase = await _produtoRepository.GetProdutoById(id);
            if (produtoBase == null)
                return null;

            var produtoLista = MapParaProdutoLista(produtoBase);
            return produtoLista;
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

            // Lógica de validação da categoria
            int idCategoria = produtoDTO.IdCategoria;

            if (Enum.IsDefined(typeof(EnumCategoria), idCategoria))
            {
                var categoriaExistente = await _produtoRepository.GetProdutosByIdCategoria((EnumCategoria)idCategoria);
                // Restante do código
                var produto = MapProdutoDtoToProduto(produtoDTO);
                return await _produtoRepository.PostProduto(produto);
            }
            else
            {
                // Lógica para lidar com valores inválidos
                // Alteração aqui para lançar uma exceção específica para ser capturada no controlador
                throw new PreconditionFailedException("A categoria informada não existe. Operação cancelada.", nameof(produtoDTO.IdCategoria));
            }
        }


        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="idProduto">O ID do produto a ser atualizado.</param>
        /// <param name="produtoPut">O produto com as informações atualizadas.</param>
        public async Task PutProduto(int idProduto, ProdutoDTO produtoPut)
        {
            var produto = await _produtoRepository.GetProdutoById(idProduto) ?? throw new ResourceNotFoundException("Produto não encontrado.");

            produto.NomeProduto = produtoPut.NomeProduto;
            produto.DescricaoProduto = produtoPut.DescricaoProduto;
            produto.ValorProduto = produtoPut.ValorProduto;
            produto.IdCategoria = produto.IdCategoria;
            produto.ImagemProduto = produtoPut.ImagemProduto;

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
        public async Task<ProdutoLista> DeleteProduto(int id)
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
                IdCategoria = produtoDto.IdCategoria,
                ImagemProduto = produtoDto.ImagemProduto
            };
        }

        /// <summary>
        /// Mapeia uma lista de objetos ProdutoBase para uma lista de objetos ProdutoLista.
        /// </summary>
        /// <param name="produtosBase">A lista de objetos ProdutoBase a ser mapeada.</param>
        /// <returns>Uma lista de objetos ProdutoLista com os mesmos dados dos objetos ProdutoBase, mais o nome da categoria.</returns>
        private static List<ProdutoLista> MapParaProdutoLista(IEnumerable<Produto> produtosBase)
        {
            // cria uma lista vazia de objetos ProdutoLista
            var produtosLista = new List<ProdutoLista>();

            // percorre a lista de objetos ProdutoBase
            foreach (var produtoBase in produtosBase)
            {
                // cria um objeto ProdutoLista com os mesmos dados do objeto ProdutoBase
                var produtoLista = MapParaProdutoLista(produtoBase);

                // adiciona o objeto ProdutoLista à lista de produtos
                produtosLista.Add(produtoLista);
            }

            // retorna a lista de objetos ProdutoLista
            return produtosLista;
        }

        /// <summary>
        /// Mapeia um objeto ProdutoBase para um objeto ProdutoLista.
        /// </summary>
        /// <param name="produtoBase">O objeto ProdutoBase a ser mapeado.</param>
        /// <returns>Um objeto ProdutoLista com os mesmos dados do objeto ProdutoBase, mais o nome da categoria.</returns>
        private static ProdutoLista MapParaProdutoLista(Produto produtoBase)
        {
            // cria um objeto ProdutoLista com os mesmos dados do objeto ProdutoBase
            return new ProdutoLista
            {
                IdProduto = produtoBase.IdProduto,
                NomeProduto = produtoBase.NomeProduto,
                ValorProduto = produtoBase.ValorProduto,
                IdCategoria = produtoBase.IdCategoria,
                DescricaoProduto = produtoBase.DescricaoProduto,
                ImagemProduto = produtoBase.ImagemProduto,
                NomeCategoria = produtoBase.Categoria?.NomeCategoria // usa a propriedade de navegação Categoria para obter o nome da categoria
            };
        }
        #endregion
    }
}
