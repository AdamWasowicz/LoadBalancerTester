namespace LBT_Api.Exceptions
{
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Occures when resource is not found
        /// </summary>
        /// <param name="msg">Exception message</param>
        public NotFoundException(string msg = "Resource not found") : base(msg)
        {

        }
    }
}
