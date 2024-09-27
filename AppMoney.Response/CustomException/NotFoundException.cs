namespace AppMoney.Respose.CustomException
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Record Not Found")
        {
        }
    }
}
