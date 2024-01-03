using System.ComponentModel;

namespace Domain.Base
{
    public enum EnumStatusPagamento
    {
        [Description("Pendente")]
        Pendente = 1,

        [Description("Aprovado")]
        Aprovado = 2,

        [Description("Reprovado")]
        Reprovado = 3,
    }
}
