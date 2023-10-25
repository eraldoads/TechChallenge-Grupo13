using System.ComponentModel.DataAnnotations;

namespace Domain.ValueObjects
{
    public class CPFAttribute : ValidationAttribute
    {
        /// <summary>
        /// Verifica se um CPF é válido.
        /// </summary>
        /// <param name="value">O CPF a ser validado.</param>
        /// <returns>Retorna verdadeiro se o CPF for válido; caso contrário, retorna falso.</returns>
        public override bool IsValid(object? value)
        {
            string? cpf = value as string;

            if (!string.IsNullOrEmpty(cpf))
            {
                // Remove os caracteres não numéricos do cpf.
                cpf = cpf.Replace(".", "").Replace("-", "");

                // Verifica se o cpf tem 11 dígitos
                if (cpf.Length != 11)
                    return false;

                // Calcula o primeiro dígito verificador.
                int soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += (10 - i) * (cpf[i] - '0');

                int resto = soma % 11;
                int digito1 = resto < 2 ? 0 : 11 - resto;

                // Verifica se o primeiro dígito verificador é igual ao décimo dígito do cpf.
                if (cpf[9] - '0' != digito1)
                    return false;

                // Calcula o segundo dígito verificador.
                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += (11 - i) * (cpf[i] - '0');

                resto = soma % 11;
                int digito2 = resto < 2 ? 0 : 11 - resto;

                // Verifica se o segundo dígito verificador é igual ao décimo primeiro dígito do cpf.
                if (cpf[10] - '0' != digito2)
                    return false;

                // Se chegou até aqui, o cpf é válido.
                return true;
            }

            // Se o valor for nulo ou vazio, retorna true, pois a validação de presença deve ser feita por outro atributo, como o Required.
            return true;
        }

    }
}
