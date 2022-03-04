using AutoMapper;
using MovieCharactersAPI.Models;
using MovieCharactersAPI.Models.DTO.Franchise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MovieCharactersAPI.Profiles
{
    public class FranchiseProfile : Profile
    {
        public FranchiseProfile()
        {
            CreateMap<Franchise, FranchiseReadDTO>()
                // Turning related movies into an array of ints
                .ForMember(cdto => cdto.Movies, opt => opt
                .MapFrom(c => c.Movies.Select(c => c.Id).ToArray()));
            CreateMap<FranchiseCreateDTO, Franchise>();
            CreateMap<FranchiseUpdateDTO, Franchise>();
        }
    }
}
