using Microsoft.EntityFrameworkCore;
using ToDo.Data.Models;

namespace ToDo.Data
{
    public sealed class ToDoContext : DbContext
    {
        public  DbSet<ToDoModel> ToDoItems { get; set; } = null!;

        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
