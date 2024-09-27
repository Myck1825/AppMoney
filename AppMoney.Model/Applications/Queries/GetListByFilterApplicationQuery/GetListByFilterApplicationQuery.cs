using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.Applications;
using MediatR;

namespace AppMoney.Model.Applications.Queries.GetListByFilterApplicationQuery
{
    public class GetListByFilterApplicationQuery : IRequest<ServerResponse<IReadOnlyList<ApplicationResponse>>>
    {
        public Guid ClientId { get; set; }
        public required string DepartmentAddress { get; set; }
    }
}