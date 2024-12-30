using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruthOrDrinkDemiBruls.Enums;

namespace TruthOrDrinkDemiBruls.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Column(TypeName ="VARCHAR(255)")]
        public AnswerType Type { get; set; }
    }
}
