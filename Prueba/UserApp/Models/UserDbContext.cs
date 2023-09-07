using Microsoft.EntityFrameworkCore;

namespace UserApp.Models
{
	public class UserDbContext : DbContext, IUserDbContext
	{
		public DbSet<User> Users { get; set; }
		public string DbPath { get; }

		public UserDbContext()
		{
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = Path.Join(path, "users.db");
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Data Source={DbPath}");
		}
	}
}
