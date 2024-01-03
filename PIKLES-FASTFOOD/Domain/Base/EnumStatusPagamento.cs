using System.ComponentModel;

namespace Domain.Base
{
    public enum EnumStatusPedido
    {
        [Description("Recebido")]
        Recebido = 1,

        [Description("Em Preparação")]
        EmPreparacao = 2,

        [Description("Pronto")]
        Pronto = 3,

        [Description("Finalizado")]
        Finalizado = 4
    }
}
