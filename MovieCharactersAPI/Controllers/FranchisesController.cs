using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCharactersAPI.Models;
using MovieCharactersAPI.Models.DTO.Franchise;

namespace MovieCharactersAPI.Controllers
{
    /// <summary>
    /// The controller holds all the API endpoints for the Franchises table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FranchisesController : ControllerBase
    {
        private readonly CharacterDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Adding context and mapper with dependency injection.
        /// </summary>
        /// <param name="context">The proper context</param>
        /// <param name="mapper">The automapper</param>
        public FranchisesController(CharacterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Franchises
        /// <summary>
        /// Fecthes all franchises from the Franchises table
        /// </summary>
        /// <returns>A collection of franchises, movies included</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseReadDTO>>> GetFranchise()
        {
            return _mapper.Map<List<FranchiseReadDTO>>(await _context.Franchises
                .Include(c => c.Movies)
                .ToListAsync());
        }

        // GET: api/Franchises/5
        /// <summary>
        /// Fetches a single franchise from the Franchises table
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <returns>the franchise matching the id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FranchiseReadDTO>> GetFranchise(int id)
        {
            //Find the franchise in the context
            var franchise = await _context.Franchises.FindAsync(id);

            if (franchise == null)
            {
                //franchise was not found
                return NotFound();
            }
            //Map franchise to read dto
            return _mapper.Map<FranchiseReadDTO>(franchise);
        }

        // PUT: api/Franchises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update the franchise in the Franchises table
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <param name="franchise">the updated franchise</param>
        /// <returns>NoContent status code if successful. Else BadRequest or NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFranchise(int id, FranchiseUpdateDTO dtoFranchise)
        {
            if (id != dtoFranchise.Id)
            {
                return BadRequest();
            }
            //Map the update dto to a franchise object
            Franchise domainFranchise = _mapper.Map<Franchise>(dtoFranchise);
            //Change the state of the franchise to modified
            _context.Entry(domainFranchise).State = EntityState.Modified;

            //Try to save the context (update the sql server)
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FranchiseExists(id))
                {
                    //Check if franchise exists
                    return NotFound();
                }
                else
                {
                    //We don't know what the problem is, just stop the program
                    throw;
                }
            }
            //NoContent is returned if nothing went wrong
            return NoContent();
        }

        // POST: api/Franchises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a franchise in the Franchises table
        /// </summary>
        /// <param name="dtoFranchise">the franchise to create</param>
        /// <returns>status code Created if successful</returns>
        [HttpPost]
        public async Task<ActionResult<Franchise>> PostFranchise(FranchiseCreateDTO dtoFranchise)
        {
            //Map the create dto to franchise object
            Franchise domainFranchise = _mapper.Map<Franchise>(dtoFranchise);
            //add to context
            _context.Franchises.Add(domainFranchise);
            //Framework takes care of the rest when saved
            await _context.SaveChangesAsync();

            //Return the movie that has been created
            return CreatedAtAction("GetFranchise", new { id = domainFranchise.Id }, _mapper.Map<FranchiseReadDTO>(domainFranchise));
        }

        // DELETE: api/Franchises/5
        /// <summary>
        /// Deletes a franchise from the Franchises table
        /// </summary>
        /// <param name="id">id of franchise to delete</param>
        /// <returns>status code NoContent if successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFranchise(int id)
        {
            var franchise = await _context.Franchises.FindAsync(id);
            if (franchise == null)
            {
                return NotFound();
            }
            //Remove franchise from context. The framework takes care of the rest when context is saved
            _context.Franchises.Remove(franchise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Adds movies to a franchise, and the franchise to the movies
        /// </summary>
        /// <param name="id">id of the franchise</param>
        /// <param name="movies">an array of movie id's</param>
        [HttpPut("{id}/movies")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCharacterInMovie(int id, int[] movies)
        {
            if (!FranchiseExists(id))
            {
                return NotFound();
            }

            //Find the correct franchise, with movies included
            Franchise franchiseToUdate = await _context.Franchises
                .Include(c => c.Movies)
                .Where(c => c.Id == id)
                .FirstAsync();

            // Loop through movies, try and assign to franchise
            var movieList = new List<Movie>();
            foreach (int movieId in movies)
            {
                Movie movie = await _context.Movies.FindAsync(movieId);
                if (movie == null)
                    // Record doesnt exist
                    return BadRequest("movie doesnt exist!");

                movieList.Add(movie);
            }
            franchiseToUdate.Movies = movieList;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();

        }

        /// <summary>
        /// Helperfunction to check if franchise exists using the context
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <returns>true if exists</returns>
        private bool FranchiseExists(int id)
        {
            return _context.Franchises.Any(e => e.Id == id);
        }
    }
}
