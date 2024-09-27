using AppMoney.Database.Constants;
using AppMoney.Model.Applications.Commands.CreateApplicationCommand;
using AppMoney.Model.Applications.Queries.GetApplicationQuery;
using AppMoney.Model.Applications.Queries.GetListByFilterApplicationQuery;
using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.Applications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.ComponentModel.DataAnnotations;

namespace AppMoney.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Create new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public ApplicationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost(Name = "CreateApplication")]
        public async Task<ServerResponse<Guid>> CreateAsync([FromBody] CreateApplicationCommand request)
        {
            if(string.IsNullOrWhiteSpace(request.DepartmentAddress))
                throw new ValidationException(nameof(request.DepartmentAddress));

            request.IpAddress = HttpContext.Items[nameof(CreateApplicationCommand.IpAddress)]?.ToString() ?? Constants.IpAddressHandlerConstants.UNKNOWNIP;

            return await _mediator.Send(request);
        }

        [HttpGet(Name = "GetById")]
        public async Task<ServerResponse<ApplicationResponse>> GetAsync([FromQuery][Required]Guid appId)
        {
            return await _mediator.Send(new GetByIdApplicationQuery { Id = appId });
        }

        [HttpGet(Name = "GetListByFilter")]
        public async Task<ServerResponse<IReadOnlyList<ApplicationResponse>>> GetListByFilterAsync([FromQuery][Required]Guid client_id, string department_address)
        {
            if (string.IsNullOrWhiteSpace(department_address))
                throw new ValidationException(nameof(department_address));

            return await _mediator.Send(new GetListByFilterApplicationQuery
            {
                ClientId = client_id,
                DepartmentAddress = department_address
            });
        }
    }
}
