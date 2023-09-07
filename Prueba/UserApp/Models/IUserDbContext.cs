using Microsoft.EntityFrameworkCore;

namespace UserApp.Models
{
	public interface IUserDbContext
	{
		string DbPath { get; }
		DbSet<User> Users { get; set; }
	}
}