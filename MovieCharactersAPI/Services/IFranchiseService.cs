using MovieCharactersAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FranchiseCharactersAPI.Services
{
    /// <summary>
    /// This service is essentially a repository, they are interchangable, still follows the same pattern. Is implemented using DI
    /// </summary>
    interface IFranchiseService
    {
        public Task<IEnumerable<Franchise>> GetAllFranchisesAsync();
        public Task<Franchise> GetSpecificFranchiseAsync(int id);
        public Task<Franchise> AddFranchiseAsync(Franchise franchise);
        public Task UpdateFranchiseAsync(Franchise franchise);
        public Task UpdateFranchiseMoviesAsync(int franchiseId, List<int> characters);
        public Task DeleteFranchiseAsync(int id);
        public bool FranchiseExists(int id);

    }
}
