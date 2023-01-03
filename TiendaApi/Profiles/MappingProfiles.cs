using AutoMapper;
using Entities.Models;
using TiendaApi.Dtos;

namespace TiendaApi.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Producto, ProductoDto>()
                .ReverseMap();
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Marca, MarcaDto>().ReverseMap();
        }
    }
}
