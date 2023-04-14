namespace LBT_Api.Exceptions
{
    public class DatabaseOperationFailedException : Exception
    {
        /// <summary>
        /// Occures when database operation failed
        /// </summary>
        /// <param name="msg">Exception message</param>
        public DatabaseOperationFailedException(string msg = "Database operation failed") : base(msg)
        {
            
        }
    }
}
