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
    [Route("api/[controller]")]
    [ApiController]
    public class FranchisesController : ControllerBase
    {
        private readonly CharacterDbContext _context;
        private readonly IMapper _mapper;

        public FranchisesController(CharacterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Franchises
        /// <summary>
        /// Gets all franchises
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
        /// Single franchise
        /// </summary>
        /// <param name="id">id of franchise</param>
        /// <returns>the franchise matching the id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FranchiseReadDTO>> GetFranchise(int id)
        {
            var franchise = await _context.Franchises.FindAsync(id);

            if (franchise == null)
            {
                return NotFound();
            }

            return _mapper.Map<FranchiseReadDTO>(franchise);
        }

        // PUT: api/Franchises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update the franchise
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

            Franchise domainFranchise = _mapper.Map<Franchise>(dtoFranchise);

            _context.Entry(domainFranchise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FranchiseExists(id))
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

        // POST: api/Franchises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a franchise
        /// </summary>
        /// <param name="dtoFranchise">the franchise to create</param>
        /// <returns>status code Created if successful</returns>
        [HttpPost]
        public async Task<ActionResult<Franchise>> PostFranchise(FranchiseCreateDTO dtoFranchise)
        {
            Franchise domainFranchise = _mapper.Map<Franchise>(dtoFranchise);
            _context.Franchises.Add(domainFranchise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFranchise", new { id = domainFranchise.Id }, _mapper.Map<FranchiseReadDTO>(domainFranchise));
        }

        // DELETE: api/Franchises/5
        /// <summary>
        /// Deletes a franchise
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

            _context.Franchises.Remove(franchise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FranchiseExists(int id)
        {
            return _context.Franchises.Any(e => e.Id == id);
        }
    }
}
