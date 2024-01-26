using System.ComponentModel;
using System.Reflection;

namespace Domain.ValueObjects
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Obtém a descrição de um valor de enum usando o atributo Description, se existir.
        /// </summary>
        /// <param name="value">O valor de enum a ser descrito.</param>
        /// <returns>A descrição do valor de enum, se existir, ou o nome do valor de enum, se não existir, ou null se o valor de enum não for definido.</returns>
        public static string? GetDescription(this Enum value)
        {
            // Obtém o tipo do enum.
            Type type = value.GetType();

            // Verifica se o valor está definido no enum.
            if (!Enum.IsDefined(type, value))
                return null;

            // Obtém o nome do valor do enum usando ToString() para evitar o valor nulo.
            string name = value.ToString();

            // Obtém o campo correspondente ao valor do enum.
            FieldInfo? field = type.GetField(name);

            // Verifica se o valor do campo correpondente ao valor do enum é nulo.
            if (field is null)
                return null;

            // Obtém o atributo Description, se existir.
            DescriptionAttribute? attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            // Retorna a descrição, se existir, ou o nome do valor do enum, se não existir.
            return attribute?.Description ?? name;
        }
    }
}
