using AppMoney.Respose.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppMoney.Respose.ApiResponse
{
    public class ServerResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
        public Code Code { get; set; }

        [JsonIgnore]
        public Exception? Exception {  get; set; }
    }
}
