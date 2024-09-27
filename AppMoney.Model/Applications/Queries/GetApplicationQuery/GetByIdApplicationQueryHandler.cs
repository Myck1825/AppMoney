using AppModey.Database.UnitOfWork;
using AppMoney.Respose;
using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.Applications;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppMoney.Model.Applications.Queries.GetApplicationQuery
{
    public class GetByIdApplicationQueryHandler : IRequestHandler<GetByIdApplicationQuery, ServerResponse<ApplicationResponse>>
    {
        private readonly ILogger<GetByIdApplicationQueryHandler> _logger;
        private readonly IUnitOfWork _dbContext;
        private readonly IMapper _mapper; 

        public GetByIdApplicationQueryHandler(ILogger<GetByIdApplicationQueryHandler> logger, 
            IUnitOfWork dbContext,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServerResponse<ApplicationResponse>> Handle(GetByIdApplicationQuery request, CancellationToken cancellationToken)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));

            var result = await _dbContext.Applications.GetByIdAsync(request.Id);

            return _mapper.Map<ServerResponse<ApplicationResponse>>(result);
        }
    }
}
