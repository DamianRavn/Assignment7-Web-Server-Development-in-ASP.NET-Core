using Microsoft.EntityFrameworkCore;
using MovieCharactersAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersAPI.Services
{
    /// <summary>
    /// an injected service/repository used by the MovieController
    /// </summary>
    public class MovieService : IMovieService
    {
        // Add context via DI
        private readonly CharacterDbContext _context;

        public MovieService(CharacterDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add movie to context and save it
        /// </summary>
        /// <param name="movie">the movie to add</param>
        /// <returns>the movie param is returned</returns>
        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            //add to context
            _context.Movies.Add(movie);
            //Framework takes care of the rest when saved
            await _context.SaveChangesAsync();
            return movie;

        }

        /// <summary>
        /// Removes movie from context and saves it
        /// </summary>
        /// <param name="id">movie to delete</param>
        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            //Remove movie from context. The framework takes care of the rest when context is saved
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Fetches all movies from context
        /// </summary>
        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies
                .Include(m => m.Characters)
                .ToListAsync();
        }

        /// <summary>
        /// finds characters from the movie id
        /// </summary>
        /// <param name="id">id of the movie</param>
        /// <returns>list of characters</returns>
        public async Task<List<Character>> GetCharactersMovieAsync(int id)
        {
            // Find the movie in the context
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                // Movie was not found
                throw new KeyNotFoundException();
            }

            return await _context.Characters.Where(c => c.Movies.Any(m => m.Id == id)).ToListAsync();
        }

        /// <summary>
        /// Fetches a single movie from context
        /// </summary>
        /// <param name="id">id of movie</param>
        /// <returns>the movie matching the id as a valuetask</returns>
        public async Task<Movie> GetSpecificMovieAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        /// <summary>
        /// Helperfunction to check if movie exists using the context
        /// </summary>
        /// <param name="id">id of movie</param>
        /// <returns>true if exists</returns>
        public bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        /// <summary>
        /// Update the movie in context and save it
        /// </summary>
        /// <param name="movie">the updated movie</param>
        public async Task UpdateMovieAsync(Movie movie)
        {
            //Change the state of the movie to modified
            _context.Entry(movie).State = EntityState.Modified;
            //Try to save the context (update the sql server)
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Assign characters to movie and updates the context, automatically updating the characters movie list as well
        /// </summary>
        /// <param name="movieId">Id of the movie to update</param>
        /// <param name="characters">the id's of characters to add</param>
        public async Task UpdateMovieCharacterAsync(int movieId, int[] characters)
        {
            //Find the correct movie, with characters included
            Movie movieToUdate = await _context.Movies
                .Include(c => c.Characters)
                .Where(c => c.Id == movieId)
                .FirstAsync();

            // Loop through characters, try and assign to movie
            foreach (int charaId in characters)
            {
                Character chara = await _context.Characters.FindAsync(charaId);
                if (chara == null)
                    // Record doesnt exist
                    throw new KeyNotFoundException();

                //Update the movie characters by adding to context. Will add, not overwrite
                movieToUdate.Characters.Add(chara);
            }
            

            //Try to save the context (update the sql server)
            await _context.SaveChangesAsync();
        }
    }
}
