using Domain.Base;
using Domain.Entities;

namespace Domain.Port.DrivenPort
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> GetProdutos();
        Task<Produto> GetProdutoById(int id);        
        Task<Produto> PostProduto(Produto produto);
        Task<int> UpdateProduto(Produto produto);
        Task<int> DeleteProduto(int id);
        Task<List<Produto>> GetProdutosByIdCategoria(EnumCategoria? idCategoria);


    }
}
