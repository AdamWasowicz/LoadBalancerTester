namespace LBT_Api.Exceptions
{
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Occures when http request is bad in any way
        /// </summary>
        /// <param name="msg">Exception message</param>
        public BadRequestException(string msg = "Bad request") : base(msg)
        {
            
        }
    }
}
