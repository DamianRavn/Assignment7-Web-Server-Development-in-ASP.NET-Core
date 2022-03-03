using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCharactersAPI.Models;
using MovieCharactersAPI.Models.DTO.Movie;

namespace MovieCharactersAPI.Controllers
{
    /// <summary>
    /// The controller holds all the API endpoints for the Movies table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly CharacterDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Adding context and mapper with dependency injection.
        /// </summary>
        /// <param name="context">The proper context</param>
        /// <param name="mapper">The automapper</param>
        public MoviesController(CharacterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Movies
        /// <summary>
        /// Fetches all movies in the Movies table
        /// </summary>
        /// <returns>A collection of movies, characters id's included</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MovieReadDTO>>> GetMovie()
        {
            return _mapper.Map<List<MovieReadDTO>>(await _context.Movies
                .Include(m => m.Characters)
                .ToListAsync());
        }

        // GET: api/Movies/5
        /// <summary>
        /// Fetches a single movie from the Movies table
        /// </summary>
        /// <param name="id">id of movie</param>
        /// <returns>the movie matching the id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieReadDTO>> GetMovie(int id)
        {
            //Find the movie in the context
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                //movie was not found
                return NotFound();
            }
            //Map movie to read dto
            return _mapper.Map<MovieReadDTO>(movie);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update the movie in the Movies table
        /// </summary>
        /// <param name="id">id of movie</param>
        /// <param name="dtoMovie">the updated movie</param>
        /// <returns>NoContent status code if successful. Else BadRequest or NotFound.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDTO dtoMovie)
        {
            if (id != dtoMovie.Id)
            {
                return BadRequest();
            }
            //Map the update dto to a movie object
            Movie domainMovie = _mapper.Map<Movie>(dtoMovie);
            //Change the state of the movie to modified
            _context.Entry(domainMovie).State = EntityState.Modified;

            //Try to save the context (update the sql server)
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Check if movie exists
                if (!MovieExists(id))
                {
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

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a movie in Movies table
        /// </summary>
        /// <param name="dtoMovie">the movie to create</param>
        /// <returns>status code Created if successful</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Movie>> PostMovie(MovieCreateDTO dtoMovie)
        {
            //Map the create dto to movie
            Movie domainMovie = _mapper.Map<Movie>(dtoMovie);
            //add to context
            _context.Movies.Add(domainMovie);
            //Framework takes care of the rest when saved
            await _context.SaveChangesAsync();

            //Return the movie that has been created
            return CreatedAtAction("GetMovie", new { id = domainMovie.Id }, _mapper.Map<MovieReadDTO>(domainMovie));
        }

        // DELETE: api/Movies/5
        /// <summary>
        /// Deletes a movie from the Movies table
        /// </summary>
        /// <param name="id">id of movie to delete</param>
        /// <returns>status code NoContent if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            //Remove movie from context. The framework takes care of the rest when context is saved
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Adds characters to a movie, and the movie to the characters
        /// </summary>
        /// <param name="id">of the movie</param>
        /// <param name="characters">an array of character id's</param>
        [HttpPut("{id}/characters")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCharacterInMovie(int id, int[] characters)
        {
            if (!MovieExists(id))
            {
                return NotFound();
            }

            //Find the correct movie, with characters included
            Movie movieToUdate = await _context.Movies
                .Include(c => c.Characters)
                .Where(c => c.Id == id)
                .FirstAsync();

            // Loop through characters, try and assign to movie
            var charaList = new List<Character>();
            foreach (int charaId in characters)
            {
                Character chara = await _context.Characters.FindAsync(charaId);
                if (chara == null)
                    // Record doesnt exist
                    return BadRequest("Character doesnt exist!");

                charaList.Add(chara);
            }
            movieToUdate.Characters = charaList;

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
        /// Helperfunction to check if movie exists using the context
        /// </summary>
        /// <param name="id">id of movie</param>
        /// <returns>true if exists</returns>
        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
