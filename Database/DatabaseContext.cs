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
}
