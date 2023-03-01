using AutoMapper;
using CatalogWebAPI.Data.Entities;
using WebAPI.Models.Dtos;

namespace WebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogItemEntity, CatalogItemDto>().ForMember("PictureUrl", options =>
            {
                options.MapFrom<CatalogItemPictureResolver, string>(c => c.PictureFileName);
            });
            CreateMap<CatalogCompanyEntity, CatalogCompanyDto>();
            CreateMap<CatalogGenreEntity, CatalogGenreDto>();
        }
    }
}
