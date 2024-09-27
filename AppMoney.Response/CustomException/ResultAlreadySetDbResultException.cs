namespace AppMoney.Respose.CustomException
{
    public class ResultAlreadySetDbResultException : Exception
    {
        public ResultAlreadySetDbResultException()
            : base("Result already setted")
        {
        }
    }
}