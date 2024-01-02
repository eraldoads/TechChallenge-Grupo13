using Domain.Base;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> GetProdutos();
        Task<Produto?> GetProdutoById(int id);
        Task<Produto> PostProduto(Produto produto);
        Task<int> UpdateProduto(Produto produto);
        Task<int> DeleteProduto(int id);
        Task<List<Categoria>> GetProdutosByIdCategoria(EnumCategoria? idCategoria);
    }
}
