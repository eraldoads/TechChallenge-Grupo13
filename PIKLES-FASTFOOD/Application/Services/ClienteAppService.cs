
using App.Application.Interfaces;
using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.ValueObjects.ResultBase;

namespace App.Application.Services
{
    public class ClienteAppService : IClienteAppService
    {
        //private readonly IExemploRepository _exemploRepository;
        //private readonly IMapper _mapper;

        //public ClienteAppService(IExemploRepository exemploRepository, IMapper mapper)
        //{
        //    _exemploRepository = exemploRepository;
        //    _mapper = mapper;
        //}

        #region [Get]
        public async Task<PagedResult<Cliente>> Get(GetCliente filtro)
        {

            //var listBD = await _exemploRepository.GetListExemplos(filtro.Nome);


            //var listVM = _mapper.Map<List<Exemplo>>(listBD.Skip(filtro._offset)
            //                                          .Take(filtro._limit)).ToList();

            //return new PagedResult<Cliente>(filtro._offset, filtro._limit, listBD.Count, listVM);

            return new PagedResult<Cliente>(0, 10, 100, new List<Cliente> { new Cliente { Nome = "Teste", CPF = "12345678910" } });
        }

        #endregion

        #region Dispose  

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //_exemploRepository.Dispose();
        }


        #endregion
    }



}
