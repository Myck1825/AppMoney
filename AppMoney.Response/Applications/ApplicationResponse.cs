namespace AppMoney.Respose.Applications
{
    public class ApplicationResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public required string Currency { get; set; }
        public required string Status { get; set; }
    }
}
