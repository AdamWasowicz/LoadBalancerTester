namespace LBT_Api.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string msg = "Resource not found") : base(msg)
        {

        }
    }
}
