using AppMoney.Model.Enums;
using AppMoney.Respose.ApiResponse;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppMoney.Model.Applications.Commands.CreateApplicationCommand
{
    public class CreateApplicationCommand : IRequest<ServerResponse<Guid>>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [StringLength(100, ErrorMessage = "Department address cannot exceed 100 characters")]
        public required string DepartmentAddress { get; set; }

        [Required]
        [Range(100, 100000, ErrorMessage = "Amount must be between 100.00 and 100000.00")]
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(CurrencyType), ErrorMessage = "Invalid cuurrency")]
        public CurrencyType Cuurrency {  get; set; }

        [JsonIgnore]
        public string? IpAddress { get; set; }
    }
}
