﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDrinkDemiBruls.Models
{
    public class Theme
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // relation to question entity
        public ICollection<Question> Questions { get; set; } = [];
    }
}
