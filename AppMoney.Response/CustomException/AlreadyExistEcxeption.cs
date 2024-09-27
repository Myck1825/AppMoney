namespace AppMoney.Respose.CustomException
{
    public class AlreadyExistEcxeption : Exception
    {
        public AlreadyExistEcxeption() : base("Record Already Exist")
        {
        }

        public AlreadyExistEcxeption(string message) : base(message)
        {
        }
    }
}
