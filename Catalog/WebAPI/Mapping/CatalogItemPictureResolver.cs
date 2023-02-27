using AutoMapper;
using CatalogWebAPI.Configurations;
using CatalogWebAPI.Data.Entities;
using Microsoft.Extensions.Options;
using WebAPI.Models.Dtos;

namespace WebAPI.Mapping
{
    public class CatalogItemPictureResolver : IMemberValueResolver<CatalogItemEntity, CatalogItemDto, string, object>
    {
        private readonly CatalogConfig _config;

        public CatalogItemPictureResolver(IOptionsSnapshot<CatalogConfig> config)
        {
            _config = config.Value;
        }

        // object cuz MapFrom() in MappingProfile.cs needs object
        public object Resolve(CatalogItemEntity source, CatalogItemDto destination, string sourceMember, object destMember, ResolutionContext context)
        {
            return $"{_config.CdnHost}/{_config.ImgUrl}/{sourceMember}";
        }
    }
}
