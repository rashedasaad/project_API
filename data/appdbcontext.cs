

using DotNetEnv;

namespace project_API.data
{
    public class appdbcontext:DbContext
    {


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseNpgsql(connectionString:Env.GetString("POSTGRES_CONNECTION_STRING"));

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        public DbSet<Products> Products { get;set; }
        public DbSet<User> Users { get;set; }
        public DbSet<Code> Codes { get;set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Categories> Category { get; set; }
        public DbSet<Order_item?> Order_Items { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Token> Tokens { get; set; }

    }
}
