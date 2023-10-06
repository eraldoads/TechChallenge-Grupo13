using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using static Domain.ValueObjects.ResultBase;

namespace App.Application.Interfaces
{
    public interface IClienteAppService
    {
        Task<PagedResult<Cliente>> Get(GetCliente filtro);
    }
}
