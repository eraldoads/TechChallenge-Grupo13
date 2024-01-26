namespace Domain.ValueObjects
{
    public class PreconditionFailedException : ArgumentException
    {
        /// <summary>
        /// Inicializa uma nova instância da classe PreconditionFailedException com uma mensagem de erro e o nome do parâmetro que causa a exceção.
        /// </summary>
        /// <param name="message">A mensagem de erro que explica o motivo da exceção.</param>
        /// <param name="paramName">O nome do parâmetro que causa a exceção.</param>
        public PreconditionFailedException(string message, string paramName) : base(message, paramName) { }
    }
}
