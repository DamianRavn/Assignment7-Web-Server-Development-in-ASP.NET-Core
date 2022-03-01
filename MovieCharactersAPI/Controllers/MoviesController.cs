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
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly CharacterDbContext _context;
        private readonly IMapper _mapper;

        public MoviesController(CharacterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Movies
        /// <summary>
        /// Get all movies
        /// </summary>
        /// <returns>A collection of movies, characters included</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieReadDTO>>> GetMovie()
        {
            return _mapper.Map<List<MovieReadDTO>>(await _context.Movies
                .Include(c => c.Characters)
                .ToListAsync());
        }

        // GET: api/Movies/5
        /// <summary>
        /// Get single movie
        /// </summary>
        /// <param name="id">id of movie</param>
        /// <returns>the movie matching the id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieReadDTO>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return _mapper.Map<MovieReadDTO>(movie);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update the movie
        /// </summary>
        /// <param name="id">id of movie</param>
        /// <param name="dtoMovie">the updated movie</param>
        /// <returns>NoContent status code if successful. Else BadRequest or NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDTO dtoMovie)
        {
            if (id != dtoMovie.Id)
            {
                return BadRequest();
            }

            Movie domainMovie = _mapper.Map<Movie>(dtoMovie);

            _context.Entry(domainMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a movie
        /// </summary>
        /// <param name="dtoMovie">the movie to create</param>
        /// <returns>status code Created if successful</returns>
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(MovieCreateDTO dtoMovie)
        {
            Movie domainMovie = _mapper.Map<Movie>(dtoMovie);
            _context.Movies.Add(domainMovie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = domainMovie.Id }, _mapper.Map<MovieReadDTO>(domainMovie));
        }

        // DELETE: api/Movies/5
        /// <summary>
        /// Deletes a movie
        /// </summary>
        /// <param name="id">id of movie to delete</param>
        /// <returns>status code NoContent if successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
