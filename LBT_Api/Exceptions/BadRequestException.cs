namespace LBT_Api.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg = "Bad request") : base(msg)
        {
            
        }
    }
}
