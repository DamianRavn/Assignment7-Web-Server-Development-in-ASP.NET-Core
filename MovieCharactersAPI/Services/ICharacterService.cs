using MovieCharactersAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCharactersAPI.Services
{
    /// <summary>
    /// This service is essentially a repository, they are interchangable, still follows the same pattern. Is implemented using DI
    /// </summary>
    interface ICharacterService
    {
        public Task<IEnumerable<Movie>> GetAllMoviesAsync();
        public Task<Movie> GetSpecificMovieAsync(int id);
        public Task<Movie> AddMovieAsync(Movie movie);
        public Task UpdateMovieAsync(Movie movie);
        public Task UpdateMovieCharacterAsync(int movieId, List<int> characters);
        public Task DeleteMovieAsync(int id);
        public bool MovieExists(int id);

    }
}
