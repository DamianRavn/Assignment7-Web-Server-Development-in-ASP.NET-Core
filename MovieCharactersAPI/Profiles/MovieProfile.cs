using AutoMapper;
using MovieCharactersAPI.Models;
using MovieCharactersAPI.Models.DTO.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MovieCharactersAPI.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieReadDTO>()
                // Turning related characters into an array of ints
                .ForMember(cdto => cdto.Characters, opt => opt
                .MapFrom(c => c.Characters.Select(c => c.Id).ToArray()))
                // Franchise in dto is FranchiseId
                .ForMember(cdto => cdto.Franchise, opt => opt
                .MapFrom(a => a.FranchiseId))
                //Two-way mapping
                .ReverseMap();

            CreateMap<MovieCreateDTO, Movie>()
                // Franchise in dto is FranchiseId
                .ForMember(cdto => cdto.FranchiseId, opt => opt
                .MapFrom(a => a.Franchise))
                //Ignore Franchise for createDTO
                .ForMember(cdto => cdto.Franchise, opt => opt.Ignore());
            CreateMap<MovieUpdateDTO, Movie>()
                // turning Franchiseid into franchise
                .ForMember(cdto => cdto.FranchiseId, opt => opt
                .MapFrom(a => a.Franchise))
                //Ignore Franchise for updateDTO
                .ForMember(cdto => cdto.Franchise, opt => opt.Ignore());
        }
    }
}
