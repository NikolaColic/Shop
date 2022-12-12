namespace Infrastructure.Exceptions
{
    [Serializable]
    public class BadRequestEntityException : Exception
    {
        public BadRequestEntityException() { }

        public BadRequestEntityException(string message) : base(message)
        {

        }
    }
}
