using Domain.Base;
using Domain.Entities;
using Domain.EntitiesDTO;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Interfaces
{
    public interface IProdutoService
    {
        Task<List<ProdutoLista>> GetProdutos();
        Task<ProdutoLista?> GetProdutoById(int id);
        Task<Produto> PostProduto(ProdutoDTO produtoDTO);
        Task PutProduto(int idProduto, ProdutoDTO produtoInput);
        Task PatchProduto(int idProduto, JsonPatchDocument<Produto> patchDoc);
        Task<ProdutoLista> DeleteProduto(int id);
        Task<List<Categoria>> GetProdutosByIdCategoria(EnumCategoria? idCategoria);
    }
}
