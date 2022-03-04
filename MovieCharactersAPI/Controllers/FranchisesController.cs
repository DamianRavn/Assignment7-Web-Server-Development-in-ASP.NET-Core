using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCharactersAPI.Models;
using MovieCharactersAPI.Models.DTO.Character;
using MovieCharactersAPI.Models.DTO.Franchise;
using MovieCharactersAPI.Models.DTO.Movie;
using MovieCharactersAPI.Services;

namespace MovieCharactersAPI.Controllers
{
    /// <summary>
    /// The controller holds all the API endpoints for the Franchises table.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FranchisesController : ControllerBase
    {
        // Add automapper via DI
        private readonly IMapper _mapper;
        // We no longer are dependent on the entire context, and it cleans up the controller code
        private readonly IFranchiseService _franchiseService;

        /// <summary>
        /// Adding service and mapper with dependency injection.
        /// </summary>
        /// <param name="franchiseService">a helper object to deal with async calls</param>
        /// <param name="mapper">The automapper</param>
        public FranchisesController(IMapper mapper, IFranchiseService franchiseService)
        {
            _franchiseService = franchiseService;
            _mapper = mapper;
        }

        /// <summary>
        /// Fecthes all franchises from the Franchises table
        /// </summary>
        /// <returns>A collection of franchises, movies included</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FranchiseReadDTO>>> GetFranchises()
        {
            return _mapper.Map<List<FranchiseReadDTO>>(await _franchiseService.GetAllFranchisesAsync());
        }

        /// <summary>
        /// Fetches a single franchise from the Franchises table
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <returns>the franchise matching the id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FranchiseReadDTO>> GetFranchise(int id)
        {
            //Find the movie in the context
            var movie = await _franchiseService.GetSpecificFranchiseAsync(id);

            if (movie == null)
            {
                //movie was not found
                return NotFound();
            }
            //Map movie to read dto
            return _mapper.Map<FranchiseReadDTO>(movie);
        }

        /// <summary>
        /// Fetches all movies in a given franchise.
        /// </summary>
        /// <param name="id">Id of the franchise.</param>
        /// <returns>The movies of the franchise matching the id.</returns>
        [HttpGet("{id}/movies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<MovieReadDTO>> GetMoviesInFranchise(int id)
        {
            //find the characters
            var movies = _franchiseService.GetMovieFranchiseAsync(id).Result;
            // Map movies to read dto
            return _mapper.Map<List<MovieReadDTO>>(movies);
        }

        /// <summary>
        /// Fetches all characters in a given franchise.
        /// </summary>
        /// <param name="id">Id of the franchise.</param>
        /// <returns>The characters of the franchise matching the id.</returns>
        [HttpGet("{id}/characters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CharacterReadDTO>>> GetCharactersInFranchise(int id)
        {
            var characters = _franchiseService.GetCharactersFranchiseAsync(id).Result;

            // Map characters to read dto.
            return _mapper.Map<List<CharacterReadDTO>>(characters);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update the franchise in the Franchises table
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <param name="franchise">the updated franchise</param>
        /// <returns>NoContent status code if successful. Else BadRequest or NotFound.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutFranchise(int id, FranchiseUpdateDTO dtoFranchise)
        {
            //Check if dtoFranchise and id correspond
            if (id != dtoFranchise.Id)
            {
                return BadRequest();
            }
            //Check if id corresponds to a movie
            if (!_franchiseService.FranchiseExists(id))
            {
                return NotFound();
            }

            //Map the update dto to a movie object
            Franchise domainFranchise = _mapper.Map<Franchise>(dtoFranchise);
            await _franchiseService.UpdateFranchiseAsync(domainFranchise);

            //NoContent is returned if nothing went wrong
            return NoContent();
        }

        /// <summary>
        /// Create a franchise in the Franchises table
        /// </summary>
        /// <param name="dtoFranchise">the franchise to create</param>
        /// <returns>status code Created if successful</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Franchise>> PostFranchise(FranchiseCreateDTO dtoFranchise)
        {
            //Map the create dto to movie
            Franchise domainFranchise = _mapper.Map<Franchise>(dtoFranchise);

            domainFranchise = await _franchiseService.AddFranchiseAsync(domainFranchise);

            //Return the movie that has been created
            return CreatedAtAction("GetFranchise", new { id = domainFranchise.Id }, _mapper.Map<FranchiseReadDTO>(domainFranchise));
        }

        /// <summary>
        /// Deletes a franchise from the Franchises table
        /// </summary>
        /// <param name="id">id of franchise to delete</param>
        /// <returns>status code NoContent if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFranchise(int id)
        {
            //Check if id corresponds to a movie
            if (!_franchiseService.FranchiseExists(id))
            {
                return NotFound();
            }

            await _franchiseService.DeleteFranchiseAsync(id);

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
        public async Task<IActionResult> UpdateMoviesInFranchise(int id, int[] movies)
        {
            //Check if id corresponds to a movie
            if (!_franchiseService.FranchiseExists(id))
            {
                return NotFound();
            }

            try
            {
                await _franchiseService.UpdateFranchiseMoviesAsync(id, movies);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("Invalid certification.");
            }

            return NoContent();

        }
    }
}
