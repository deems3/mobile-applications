using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruthOrDrinkDemiBruls.Enums;

namespace TruthOrDrinkDemiBruls.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;

        // column VARCHAR because enum normally indicates integers and now it has to be a status "string" so its readable in DB.
        [Column(TypeName ="VARCHAR(255)")]
        public QuestionIntensity Intensity { get; set; }

        // relation to theme entity
        public ICollection<Theme> Themes { get; set; } = [];

        // relation to player entity no icollection because the question is linked to one player (optional)
        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        public ICollection<GameQuestion> GameQuestions { get; set; } = [];
    }
}
