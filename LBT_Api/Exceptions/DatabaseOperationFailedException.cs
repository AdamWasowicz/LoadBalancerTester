namespace LBT_Api.Exceptions
{
    public class DatabaseOperationFailedException : Exception
    {
        public DatabaseOperationFailedException(string msg = "Database operation failed") : base(msg)
        {
            
        }
    }
}
