using Microsoft.AspNetCore.Mvc;
using MovieCharactersAPI.Models;
using MovieCharactersAPI.Models.DTO.Character;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCharactersAPI.Services
{
    /// <summary>
    /// This service is essentially a repository, they are interchangable, still follows the same pattern. Is implemented using DI
    /// </summary>
    public interface IFranchiseService
    {
        /// <summary>
        /// Fetches all franchises from context
        /// </summary>
        public Task<IEnumerable<Franchise>> GetAllFranchisesAsync();
        /// <summary>
        /// Fetches a single franchise from context
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <returns>the franchise matching the id</returns>
        public Task<Franchise> GetSpecificFranchiseAsync(int id);
        /// <summary>
        /// Add franchise to context and save it
        /// </summary>
        /// <param name="franchise">the franchise to add</param>
        /// <returns>the franchise param is returned</returns>
        public Task<Franchise> AddFranchiseAsync(Franchise franchise);
        /// <summary>
        /// Update the franchise in context and save it
        /// </summary>
        /// <param name="franchise">the updated franchise</param>
        public Task UpdateFranchiseAsync(Franchise franchise);
        /// <summary>
        /// Assign movies to franchise and updates the context, automatically updating the movie franchise reference as well
        /// </summary>
        /// <param name="franchiseId">Id of the franchise to update</param>
        /// <param name="characters">the id's of characters to add</param>
        public Task UpdateFranchiseMoviesAsync(int franchiseId, int[] movies);

        /// <summary>
        /// finds movies from the franchise id
        /// </summary>
        /// <param name="id">id of the franchise</param>
        /// <returns>list of movies</returns>
        public Task<List<Movie>> GetMovieFranchiseAsync(int id);

        /// <summary>
        /// Fetches all characters in a given franchise.
        /// </summary>
        /// <param name="id">Id of the franchise.</param>
        /// <returns>The characters of the franchise matching the id.</returns>
        public Task<List<Character>> GetCharactersFranchiseAsync(int id);

        /// <summary>
        /// Removes franchise from context and saves it
        /// </summary>
        /// <param name="id">franchise to delete</param>
        public Task DeleteFranchiseAsync(int id);
        /// <summary>
        /// Helperfunction to check if franchise exists using the context
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <returns>true if exists</returns>
        public bool FranchiseExists(int id);

    }
}
