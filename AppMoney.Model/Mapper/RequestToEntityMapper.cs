using AppMoney.Database.Entities;
using AppMoney.Model.Applications.Commands.CreateApplicationCommand;
using AutoMapper;

namespace AppMoney.Model.Mapper
{
    public class RequestToEntityMapper : Profile
    {
        public RequestToEntityMapper()
        {
            CreateMap<CreateApplicationCommand, Application>()
                .ForMember(d=>d.ClientIp, d=>d.MapFrom(m=>m.IpAddress))
                .ForMember(d=>d.Currency, d=>d.MapFrom(m=>m.Cuurrency.ToString()));
        }
    }
}
