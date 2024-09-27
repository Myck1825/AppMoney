using AppMoney.Database.Entities;
using AppMoney.Respose.Applications;
using AutoMapper;

namespace AppMoney.Database.Mapper
{
    public class EntityToResponseMapper : Profile
    {
        public EntityToResponseMapper() 
        {
            CreateMap<Application, ApplicationResponse>();
        }
    }
}
