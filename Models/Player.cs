using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDrinkDemiBruls.Models
{
    internal class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // relation to game entity
        public ICollection<Game> Games { get; set; } = [];

        // relation to question entity, icollection because a player is able to have one or more questions
        public ICollection<Question> Questions { get; set; } = [];
        public ICollection<GameQuestion> GameQuestions { get; set; } = [];
    }
}
