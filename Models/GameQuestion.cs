using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDrinkDemiBruls.Models
{
    public class GameQuestion
    {
        public int Id { get; set; }

        public int GameId {  get; set; }
        public Game Game { get; set; } = null!;

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public int? PlayerId { get; set; }
        public Player? Player { get; set; }
        // TODO : Answer
    }
}
