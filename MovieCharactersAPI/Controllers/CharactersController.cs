using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using MovieCharactersAPI.Models;
using AutoMapper;
using MovieCharactersAPI.Models.DTO.Character;

namespace MovieCharactersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly CharacterDbContext _context;
        private readonly IMapper _mapper;

        // Adding context and mapper with dependency injection.
        public CharactersController(CharacterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Fetches all characters in the database.
        /// </summary>
        /// <returns>A collection of characters.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CharacterReadDTO>>> GetCharacter()
        {
            return _mapper.Map<List<CharacterReadDTO>>(await _context.Characters
                .Include(c => c.Movies)
                .ToListAsync());
        }

        /// <summary>
        /// Fetches a specific character in the database.
        /// </summary>
        /// <param name="id">The id of the character to fetch.</param>
        /// <returns>The character, if it exists. Otherwise, the NotFound response.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CharacterReadDTO>> GetCharacter(int id)
        {
            var character = await _context.Characters.FindAsync(id);

            if (character == null)
            {
                return NotFound();
            }

            return _mapper.Map<CharacterReadDTO>(character);
        }

        /// <summary>
        /// Updates a specific character with the values of the given character object.
        /// </summary>
        /// <param name="id">The id of the character to update.</param>
        /// <param name="dtoCharacter">A character object containing the updated values.</param>
        /// <returns>Nothing if the update succeeds. BadRequest if the given id doesn't match the id of the character. NotFound if the character doesn't exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutCharacter(int id, CharacterUpdateDTO dtoCharacter)
        {
            if (id != dtoCharacter.Id)
            {
                return BadRequest();
            }

            Character domainCharacter = _mapper.Map<Character>(dtoCharacter);

            _context.Entry(domainCharacter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
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

        /// <summary>
        /// Creates a new character based on the given character object.
        /// </summary>
        /// <param name="dtoCharacter">The character object.</param>
        /// <returns>The newly created character.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Character>> PostCharacter(CharacterCreateDTO dtoCharacter)
        {
            Character domainCharacter = _mapper.Map<Character>(dtoCharacter);
            _context.Characters.Add(domainCharacter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharacter", new { id = domainCharacter.Id },
                                   _mapper.Map<CharacterReadDTO>(domainCharacter));
        }

        /// <summary>
        /// Deletes the character with the given id.
        /// </summary>
        /// <param name="id">The id of the character to delete.</param>
        /// <returns>Nothing if the deletion succeeds. NotFound if no character with the given id exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Helper function, which checks if a character with the given id exists.
        /// </summary>
        /// <param name="id">The id to check for.</param>
        /// <returns>True if a character with that id exists. False otherwise.</returns>
        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }
    }
}
