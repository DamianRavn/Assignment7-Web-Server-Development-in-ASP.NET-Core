using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieCharactersAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MovieCharactersAPI.Models.DTO.Character;

namespace MovieCharactersAPI.Services
{
    /// <summary>
    /// an injected service/repository used by the FranchiseController
    /// </summary>
    public class FranchiseService : IFranchiseService
    {
        // Add context via DI
        private readonly CharacterDbContext _context;

        public FranchiseService(CharacterDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add franchise to context and save it
        /// </summary>
        /// <param name="franchise">the franchise to add</param>
        /// <returns>the franchise param is returned</returns>
        public async Task<Franchise> AddFranchiseAsync(Franchise franchise)
        {
            //add to context
            _context.Franchises.Add(franchise);
            //Framework takes care of the rest when saved
            await _context.SaveChangesAsync();
            return franchise;

        }

        /// <summary>
        /// Removes franchise from context and saves it
        /// </summary>
        /// <param name="id">franchise to delete</param>
        public async Task DeleteFranchiseAsync(int id)
        {
            var franchise = await _context.Franchises.FindAsync(id);
            //Remove franchise from context. The framework takes care of the rest when context is saved
            _context.Franchises.Remove(franchise);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Fetches all franchises from context
        /// </summary>
        public async Task<IEnumerable<Franchise>> GetAllFranchisesAsync()
        {
            return await _context.Franchises
                .Include(m => m.Movies)
                .ToListAsync();
        }

        /// <summary>
        /// finds movies from the franchise id
        /// </summary>
        /// <param name="id">id of the franchise</param>
        /// <returns>list of movies</returns>
        public async Task<List<Movie>> GetMovieFranchiseAsync(int id)
        {
            // Find the franchise in the context
            var franchise = await _context.Franchises.FindAsync(id);

            if (franchise == null)
            {
                // franchise was not found
                throw new KeyNotFoundException();
            }

            
            return await _context.Movies.Where(m => m.FranchiseId == id).ToListAsync();
        }

        /// <summary>
        /// Fetches a single franchise from context
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <returns>the franchise matching the id as a valuetask</returns>
        public async Task<Franchise> GetSpecificFranchiseAsync(int id)
        {
            return await _context.Franchises.FindAsync(id);
        }

        /// <summary>
        /// Helperfunction to check if franchise exists using the context
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <returns>true if exists</returns>
        public bool FranchiseExists(int id)
        {
            return _context.Franchises.Any(e => e.Id == id);
        }

        /// <summary>
        /// Update the franchise in context and save it
        /// </summary>
        /// <param name="franchise">the updated franchise</param>
        public async Task UpdateFranchiseAsync(Franchise franchise)
        {
            //Change the state of the franchise to modified
            _context.Entry(franchise).State = EntityState.Modified;
            //Try to save the context (update the sql server)
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Assign characters to franchise and updates the context, automatically updating the characters franchise list as well
        /// </summary>
        /// <param name="franchiseId">Id of the franchise to update</param>
        /// <param name="characters">the id's of characters to add</param>
        public async Task UpdateFranchiseMoviesAsync(int franchiseId, int[] movies)
        {
            //Find the correct franchise, with movies included
            Franchise franchiseToUdate = await _context.Franchises
                .Include(c => c.Movies)
                .Where(c => c.Id == franchiseId)
                .FirstAsync();

            // Loop through movies, try and assign to franchise
            foreach (int movieId in movies)
            {
                Movie movie = await _context.Movies.FindAsync(movieId);
                if (movie == null)
                    // Record doesnt exist
                    throw new KeyNotFoundException();

                franchiseToUdate.Movies.Add(movie);
            }

            //Try to save the context (update the sql server)
            await _context.SaveChangesAsync();
        }

        public async Task<List<Character>> GetCharactersFranchiseAsync(int id)
        {
            // Find the franchise in the context
            var franchise = await _context.Franchises.FindAsync(id);

            // Get the ids of all the movies in the franchise.
            var movieIds = await _context.Movies
                .Where(m => m.FranchiseId == id)
                .Select(m => m.Id)
                .ToListAsync();

            var characters = new List<Character>();

            // Loop over the movie ids and get the characters in each movie.
            foreach (var movieId in movieIds)
            {
                characters.AddRange(await _context.Characters
                    .Where(c => c.Movies
                    .Any(m => m.Id == movieId))
                    .ToListAsync());
            }

            // Remove duplicate characters.
            return characters.Distinct().ToList();
        }
    }
}
