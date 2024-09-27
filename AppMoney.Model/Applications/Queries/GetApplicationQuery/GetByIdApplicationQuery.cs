using AppMoney.Model.Enums;
using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.Applications;
using MediatR;

namespace AppMoney.Model.Applications.Queries.GetApplicationQuery
{
    public class GetByIdApplicationQuery : IRequest<ServerResponse<ApplicationResponse>>
    {
        public Guid Id { get; set; }
    }
}
