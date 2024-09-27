namespace AppMoney.Database.Entities
{
    public class Application
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public required string Currency { get; set; }
        public required string Status { get; set; }
        public required string DepartmentAddress {  get; set; }
        public Guid ClientId { get; set; }
        public required string ClientIp { get; set; }
        public int ErrorNumber { get; set; }
    }
}
