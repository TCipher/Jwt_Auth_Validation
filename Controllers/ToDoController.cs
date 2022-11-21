using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleToDoApi.Data;
using SimpleToDoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SimpleToDoApi.Controllers
{
    [Route("api/[controller]")]//api/todo
    [ApiController]
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ToDoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ToDoController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var toDoItems = await _context.items.ToListAsync();
            return Ok(toDoItems);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(ToDoItem data)
        {
            if (ModelState.IsValid)
            {
                await _context.items.AddAsync(data);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItem", new { data.Id }, data);
            }
            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _context.items.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateItem(int id, ToDoItem item)
        {
            if (id != item.Id)
                return BadRequest();
            var existItem = await _context.items.FirstOrDefaultAsync(x => x.Id == id);
            if (existItem == null)
                return NotFound();
            existItem.Title = item.Title;
            existItem.Description = item.Description;
            existItem.Status = item.Status;

            //implement the changes in the database
            await _context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var existitem = await _context.items.FirstOrDefaultAsync(x => x.Id == id);
            if (existitem == null)
                return NotFound();
            _context.items.Remove(existitem);
            await _context.SaveChangesAsync();
            return Ok(existitem);
        }
    }

    
}
