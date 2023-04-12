namespace LBT_Api.Exceptions
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException(string msg = "Model is invalid") : base(msg)
        {
            
        }
    }
}
