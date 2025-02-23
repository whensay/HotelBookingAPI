﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBookingAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelBookingContext _context;

        public HotelsController(HotelBookingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            return await _context.Hotels.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return hotel;
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.HotelId }, hotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                return BadRequest();
            }

            _context.Entry(hotel).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Hotels.Any(e => e.HotelId == id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
