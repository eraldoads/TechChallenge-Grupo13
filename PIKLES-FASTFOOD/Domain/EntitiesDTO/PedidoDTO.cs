using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Domain.EntitiesDTO
{
    //[SwaggerSchemaFilter(typeof(PedidoDTOSchemaFilter))]
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public DateTimeOffset DataPedido { get; set; }
        public string? StatusPedido { get; set; }
        public string? NomeCliente { get; set; }
        public float ValorTotalPedido { get; set; }
        public List<ComboDTO>? Combo { get; set; }
    }
}
