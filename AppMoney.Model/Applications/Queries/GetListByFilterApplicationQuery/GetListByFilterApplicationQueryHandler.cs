using AppModey.Database.UnitOfWork;
using AppMoney.Model.Applications.Queries.GetApplicationQuery;
using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.Applications;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppMoney.Model.Applications.Queries.GetListByFilterApplicationQuery
{
    public class GetListByFilterApplicationQueryHandler : IRequestHandler<GetListByFilterApplicationQuery, ServerResponse<IReadOnlyList<ApplicationResponse>>>
    {
        private readonly ILogger<GetByIdApplicationQueryHandler> _logger;
        private readonly IUnitOfWork _dbContext;
        private readonly IMapper _mapper;

        public GetListByFilterApplicationQueryHandler(ILogger<GetByIdApplicationQueryHandler> logger,
            IUnitOfWork dbContext,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServerResponse<IReadOnlyList<ApplicationResponse>>> Handle(GetListByFilterApplicationQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var applicationList = await _dbContext.Applications.GetByFilterAsync(request.ClientId, request.DepartmentAddress);

            return _mapper.Map<ServerResponse<IReadOnlyList<ApplicationResponse>>>(applicationList);
        }
    }
}
