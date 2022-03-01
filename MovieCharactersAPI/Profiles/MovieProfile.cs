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
                // Turning related characters into arrays
                .ForMember(cdto => cdto.Characters, opt => opt
                .MapFrom(c => c.Characters.Select(c => c.Id).ToArray()))
                // turning Franchiseid into franchise
                .ForMember(cdto => cdto.Franchise, opt => opt
                .MapFrom(a => a.FranchiseId))
                .ReverseMap();

            CreateMap<MovieCreateDTO, Movie>();
            CreateMap<MovieUpdateDTO, Movie>()
                // turning Franchiseid into franchise
                .ForMember(cdto => cdto.Franchise, opt => opt
                .MapFrom(a => a.Franchise))
                .ReverseMap();
        }
    }
}
