using DiplomApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Reflection.Emit;

namespace DiplomApi.Data.Database
{
    public class DatabaseContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Models.Object> Objects { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<MessageData> MessageData { get; set; }
        public DbSet<Apartament> Apartament { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
            Database.EnsureCreated();
         
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseSqlServer(Program.Configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
