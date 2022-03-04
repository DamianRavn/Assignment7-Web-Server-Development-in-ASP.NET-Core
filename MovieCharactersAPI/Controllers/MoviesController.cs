using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCharactersAPI.Models;
using MovieCharactersAPI.Models.DTO.Character;
using MovieCharactersAPI.Models.DTO.Movie;
using MovieCharactersAPI.Services;

namespace MovieCharactersAPI.Controllers
{
    /// <summary>
    /// The controller holds all the API endpoints for the Movies table.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // Add automapper via DI
        private readonly IMapper _mapper;
        // We no longer are dependent on the entire context, and it cleans up the controller code
        private readonly IMovieService _movieService;

        /// <summary>
        /// Adding service and mapper with dependency injection.
        /// </summary>
        /// <param name="movieService">a helper object to deal with async calls</param>
        /// <param name="mapper">The automapper</param>
        public MoviesController(IMapper mapper, IMovieService movieService)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        /// <summary>
        /// Fetches all movies in the Movies table.
        /// </summary>
        /// <returns>A collection of movies, characters id's included.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MovieReadDTO>>> GetMovies()
        {
            return _mapper.Map<List<MovieReadDTO>>(await _movieService.GetAllMoviesAsync());
        }

        /// <summary>
        /// Fetches a single movie from the Movies table.
        /// </summary>
        /// <param name="id">Id of movie.</param>
        /// <returns>The movie matching the id.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieReadDTO>> GetMovie(int id)
        {
            //Find the movie in the context
            var movie = await _movieService.GetSpecificMovieAsync(id);

            if (movie == null)
            {
                //movie was not found
                return NotFound();
            }
            //Map movie to read dto
            return _mapper.Map<MovieReadDTO>(movie);
        }

        /// <summary>
        /// Fetches all characters in a given movie.
        /// </summary>
        /// <param name="id">Id of the movie.</param>
        /// <returns>The characters of the movie matching the id.</returns>
        [HttpGet("{id}/characters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CharacterReadDTO>>> GetCharactersInMovie(int id)
        {
            //find the characters
            var characters = _movieService.GetCharactersMovieAsync(id).Result;
            //Map franchise to read dto
            return _mapper.Map<List<CharacterReadDTO>>(characters);
        }

        /// <summary>
        /// Update the movie in the Movies table.
        /// </summary>
        /// <param name="id">Id of movie.</param>
        /// <param name="dtoMovie">the updated movie</param>
        /// <returns>NoContent status code if successful. Else BadRequest or NotFound.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDTO dtoMovie)
        {
            //Check if dtoMovie and id correspond
            if (id != dtoMovie.Id)
            {
                return BadRequest();
            }
            //Check if id corresponds to a movie
            if (!_movieService.MovieExists(id))
            {
                return NotFound();
            }

            //Map the update dto to a movie object
            Movie domainMovie = _mapper.Map<Movie>(dtoMovie);
            await _movieService.UpdateMovieAsync(domainMovie);

            //NoContent is returned if nothing went wrong
            return NoContent();
        }

        /// <summary>
        /// Create a movie in Movies table.
        /// </summary>
        /// <param name="dtoMovie">The movie to create.</param>
        /// <returns>Status code Created if successful.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Movie>> PostMovie(MovieCreateDTO dtoMovie)
        {
            //Map the create dto to movie
            Movie domainMovie = _mapper.Map<Movie>(dtoMovie);

            domainMovie = await _movieService.AddMovieAsync(domainMovie);

            //Return the movie that has been created
            return CreatedAtAction("GetMovie", new { id = domainMovie.Id }, _mapper.Map<MovieReadDTO>(domainMovie));
        }

        /// <summary>
        /// Deletes a movie from the Movies table.
        /// </summary>
        /// <param name="id">Id of movie to delete.</param>
        /// <returns>Status code NoContent if successful.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            //Check if id corresponds to a movie
            if (!_movieService.MovieExists(id))
            {
                return NotFound();
            }

            await _movieService.DeleteMovieAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Adds characters to a movie, and the movie to the characters.
        /// </summary>
        /// <param name="id">If of the movie.</param>
        /// <param name="characters">An array of character ids.</param>
        [HttpPut("{id}/characters")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCharacterInMovie(int id, int[] characters)
        {
            //Check if id corresponds to a movie
            if (!_movieService.MovieExists(id))
            {
                return NotFound();
            }

            try
            {
                await _movieService.UpdateMovieCharacterAsync(id, characters);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("Invalid certification.");
            }

            return NoContent();
        }
    }
}
