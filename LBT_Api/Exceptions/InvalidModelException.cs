namespace LBT_Api.Exceptions
{
    public class InvalidModelException : Exception
    {
        /// <summary>
        /// Occures when model validation fails
        /// </summary>
        /// <param name="msg">Exception message</param>
        public InvalidModelException(string msg = "Model is invalid") : base(msg)
        {
            
        }
    }
}
