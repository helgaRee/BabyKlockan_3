using AutoMapper;
using BabyKlockan_3.Entities;
using BabyKlockan_3.Models;

namespace BabyKlockan_3.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //mappar Contraction <->ContractionModel
        CreateMap<Contraction, ContractionModel>();
    }
}
