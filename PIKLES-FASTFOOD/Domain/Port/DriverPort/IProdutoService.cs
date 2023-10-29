using Domain.Base;
using Domain.Entities;
using Domain.EntitiesDTO;
using Microsoft.AspNetCore.JsonPatch;

namespace Domain.Port.Services
{
    public interface IProdutoService
    {
        Task<List<Produto>> GetProdutos();
        Task<Produto?> GetProdutoById(int id);
        Task<Produto> PostProduto(ProdutoDTO produtoDTO);
        Task PutProduto(int idProduto, ProdutoDTO produtoInput);
        Task PatchProduto(int idProduto, JsonPatchDocument<Produto> patchDoc);
        Task<Produto> DeleteProduto(int id);
        Task<List<Categoria>> GetProdutosByIdCategoria(EnumCategoria? idCategoria);
    }
}
