namespace AppMoney.Respose.CustomException
{
    public class SqlDataException : Exception
    {
        public SqlDataException(string message) : base(message)
        {
        }
    }
}
