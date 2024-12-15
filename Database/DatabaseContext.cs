using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDrinkDemiBruls.Database;

internal class DatabaseContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionDb = $"Filename={Helper.GetDbPath("truth_or_drink_demi.db")}";
        optionsBuilder.UseSqlite(connectionDb);
    }
}
