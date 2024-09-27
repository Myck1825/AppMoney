using AppMoney.Database.Entities;
using AppMoney.Respose;
using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.Applications;
using AutoMapper;

namespace AppModey.Database.Mapper
{
    public class DbResultToServerResponseMapper : Profile
    {

        public DbResultToServerResponseMapper()
        {
            CreateMap<DbResult<Guid>, ServerResponse<Guid>>()
                .AfterMap((src, dest) => HandleException(dest));
            CreateMap<DbResult<Application>, ServerResponse<ApplicationResponse>>()
                .AfterMap((src, dest) => HandleException(dest));
            CreateMap<DbResult<IReadOnlyList<Application>>, ServerResponse<IReadOnlyList<ApplicationResponse>>>()
                .AfterMap((src, dest) => HandleException(dest));
        }

        #region Private helpers
        private void HandleException<T>(ServerResponse<T> destination)
        {
            if (destination.Exception != null)
            {
                throw destination.Exception;
            }
        }
        #endregion
    }
}
