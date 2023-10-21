using Domain.Base;
using Domain.Entities;
using Domain.EntitiesDTO;

namespace Domain.Port.Services
{
    public interface IProdutoService
    {
        Task<List<Produto>> GetProdutos();
        Task<Produto> GetProdutoById(int? id);        
        Task<Produto> PostProduto(ProdutoDTO produtoDTO);
        Task UpdateProduto(Produto produto);        
        Task<int> DeleteProduto(int id);
        Task<List<Produto>> GetProdutosByIdCategoria(EnumCategoria? idCategoria);
    }
}
