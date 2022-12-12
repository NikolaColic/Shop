using AutoMapper;
using Data.Entities;
using Enigmatry.Grpc;

namespace Vendor.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Article, ArticleDtoGrpc>();
        } 
    }
}
