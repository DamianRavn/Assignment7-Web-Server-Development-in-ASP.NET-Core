using MovieCharactersAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCharactersAPI.Services
{
    /// <summary>
    /// This service is essentially a repository, they are interchangable, still follows the same pattern. Is implemented using DI
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Fetches all movies from context
        /// </summary>
        public Task<IEnumerable<Movie>> GetAllMoviesAsync();
        /// <summary>
        /// Fetches a single movie from context
        /// </summary>
        /// <param name="id">id of movie</param>
        /// <returns>the movie matching the id</returns>
        public Task<Movie> GetSpecificMovieAsync(int id);
        /// <summary>
        /// Add movie to context and save it
        /// </summary>
        /// <param name="movie">the movie to add</param>
        /// <returns>the movie param is returned</returns>
        public Task<Movie> AddMovieAsync(Movie movie);
        /// <summary>
        /// Update the movie in context and save it
        /// </summary>
        /// <param name="movie">the updated movie</param>
        public Task UpdateMovieAsync(Movie movie);
        /// <summary>
        /// Assign characters to movie and updates the context, automatically updating the characters movie list as well
        /// </summary>
        /// <param name="movieId">Id of the movie to update</param>
        /// <param name="characters">the id's of characters to add</param>
        public Task UpdateMovieCharacterAsync(int movieId, int[] characters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<Character>> GetCharactersMovieAsync(int id);

        /// <summary>
        /// Removes movie from context and saves it
        /// </summary>
        /// <param name="id">movie to delete</param>
        public Task DeleteMovieAsync(int id);
        /// <summary>
        /// Helperfunction to check if movie exists using the context
        /// </summary>
        /// <param name="id">id of movie</param>
        /// <returns>true if exists</returns>
        public bool MovieExists(int id);

    }
}
