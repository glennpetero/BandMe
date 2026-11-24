using Microsoft.EntityFrameworkCore;

namespace BandMe.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Band> Bands => Set<Band>();
    public DbSet<Interest> Interests => Set<Interest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial interest tags so the questionnaire has options on Day 1
        modelBuilder.Entity<Interest>().HasData(
            new Interest { Id = 1, Name = "C# & .NET", Category = "Tech" },
            new Interest { Id = 2, Name = "Web Development", Category = "Tech" },
            new Interest { Id = 3, Name = "Game Dev", Category = "Tech" },
            new Interest { Id = 4, Name = "Rock & Indie Music", Category = "Music" },
            new Interest { Id = 5, Name = "Jazz & Blues", Category = "Music" },
            new Interest { Id = 6, Name = "Board Games & RPGs", Category = "Hobbies" },
            new Interest { Id = 7, Name = "Hiking & Outdoors", Category = "Outdoors" },
            new Interest { Id = 8, Name = "Book Club", Category = "Lifestyle" }
        );
    }
}
