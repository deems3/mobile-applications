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
        public string Code { get; set; } = null!;

        public ICollection<Player> Players { get; set; } = [];

        public ICollection<GameQuestion> GameQuestions { get; set; } = [];
    }
}
