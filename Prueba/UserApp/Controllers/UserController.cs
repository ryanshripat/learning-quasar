using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UserApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserDbContext context;
		public UserController(DbContext context) 
		{
			this.context = (UserDbContext)context;
		}

		// GET: api/<UserController>
		[HttpGet]
		public ActionResult<IEnumerable<User>> Get()
		{
			return context.Users?.ToList();
		}

		// GET api/<UserController>/5
		[HttpGet("{id}")]
		public ActionResult<User> Get(int id)
		{
			var foundUser = context.Users.FirstOrDefault(u => u.Id == id);
			if (foundUser != null)
			{
				return foundUser;
			}
			else return NotFound();
		}

		// POST api/<UserController>
		[HttpPost]
		public IActionResult Post([FromBody][Required] User user)
		{
			var existingUser = context.Users.FirstOrDefault(u => u.Id == user.Id);
			if (existingUser != null)
			{
				existingUser.Name = user.Name;
				existingUser.Email = user.Email;
				context.Update(existingUser);
				var numberOfUsersUpdated = context.SaveChanges();
				if (numberOfUsersUpdated == 1)
				{
					return NoContent();
				}
				else return StatusCode(StatusCodes.Status500InternalServerError);
			}
			else return NotFound();
		}

		// PUT api/<UserController>/5
		[HttpPut()]
		public IActionResult Put(User newUser)
		{
			newUser.Id = new Random().Next();
			context.Add(newUser);
			var numberOfUsersAdded = context.SaveChanges();
			if (numberOfUsersAdded == 1)
			{
				return Created($"/User/{newUser.Id}", newUser);
			}
			else return StatusCode(StatusCodes.Status500InternalServerError);
		}

		// DELETE api/<UserController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var existingUser = context.Users.FirstOrDefault(u => u.Id == id);
			if (existingUser != null)
			{
				context.Remove(existingUser);
				var numberOfUsersRemoved = context.SaveChanges();
				if (numberOfUsersRemoved == 1)
				{
					return NoContent();
				}
				else return StatusCode(StatusCodes.Status500InternalServerError);
			}
			else return NotFound();
		}
	}
}
