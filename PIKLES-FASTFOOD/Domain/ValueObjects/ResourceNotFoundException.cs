namespace Domain.ValueObjects
{
    public class ResourceNotFoundException : Exception
    {
        /// <summary>
        /// Construtor para a classe ResourceNotFoundException.
        /// </summary>
        /// <param name="message">A mensagem de erro a ser associada à exceção.</param>
        public ResourceNotFoundException(string message) : base(message) { }

    }
}
