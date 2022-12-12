namespace Infrastructure.Exceptions
{
    [Serializable]
    public class UnproccesableException : Exception
    {
        public UnproccesableException() { }

        public UnproccesableException(string message) : base(message)
        {

        }
    }
}
