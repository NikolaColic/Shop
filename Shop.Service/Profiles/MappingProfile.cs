using AutoMapper;
using Data.Entities;
using Enigmatry.Grpc;

namespace Shop.Service.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ArticleDtoGrpc, Article>();
        }
    }
}
