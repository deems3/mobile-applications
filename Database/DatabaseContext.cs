using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Theme> Themes { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<GameQuestion> GameQuestions { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionDb = $"Filename={Helper.GetDbPath("truth_or_drink_demi.db")}";
        optionsBuilder.UseSqlite(connectionDb);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Theme>()
            .HasData(
                new Theme
                {
                    Id = 1,
                    Name = "Romantiek & relaties"
                },
                new Theme
                {
                    Id = 2,
                    Name = "Reizen & avontuur"
                },
                new Theme
                {
                    Id = 3,
                    Name = "Persoonlijke geheimen"
                },
                new Theme
                {
                    Id = 4,
                    Name = "Hobby's & interesses"
                },
                new Theme
                {
                    Id = 5,
                    Name = "Familie & vriendschap"
                },
                new Theme
                {
                    Id = 6,
                    Name = "Levenskeuzes & beslissingen"
                },
                new Theme
                {
                    Id = 7,
                    Name = "Eten & drank"
                }
            );
    }
}
