using AutoMapper;
using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmApp.Contracts.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarDTO>()
                .ForMember(dest =>
                    dest.BodyType,
                    opt => opt.MapFrom(src => src.BodyType))
                .ForMember(dest =>
                    dest.Color,
                    opt => opt.MapFrom(src => src.Color))
                .ForMember(dest =>
                    dest.CreatedAt,
                    opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest =>
                    dest.Displacement,
                    opt => opt.MapFrom(src => src.Displacement))
                .ForMember(dest =>
                    dest.Drive,
                    opt => opt.MapFrom(src => src.Drive))
                .ForMember(dest =>
                    dest.Engine,
                    opt => opt.MapFrom(src => src.Engine))
                .ForMember(dest =>
                    dest.FuelType,
                    opt => opt.MapFrom(src => src.FuelType))
                .ForMember(dest =>
                    dest.Interior,
                    opt => opt.MapFrom(src => src.Interior))
                .ForMember(dest =>
                    dest.MakeId,
                    opt => opt.MapFrom(src => src.MakeId))
                .ForMember(dest =>
                    dest.ManufactureDate,
                    opt => opt.MapFrom(src => src.ManufactureDate))
                .ForMember(dest =>
                    dest.Power,
                    opt => opt.MapFrom(src => src.Power))
                .ForMember(dest =>
                    dest.Series,
                    opt => opt.MapFrom(src => src.Series))
                .ForMember(dest =>
                    dest.Steering,
                    opt => opt.MapFrom(src => src.Steering))
                .ForMember(dest =>
                    dest.Transmission,
                    opt => opt.MapFrom(src => src.Transmission))
                .ForMember(dest =>
                    dest.Vin,
                    opt => opt.MapFrom(src => src.Vin))
                .ReverseMap();

        }
    }
}
