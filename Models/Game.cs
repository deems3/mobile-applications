using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDrinkDemiBruls.Models
{
    internal class Game
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public ICollection<Player> Players { get; set; } = [];

        public ICollection<Question> Questions { get; set; } = [];
    }
}
