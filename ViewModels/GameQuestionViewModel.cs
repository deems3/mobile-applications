using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.ViewModels
{
    public partial class GameQuestionViewModel : ObservableObject
    {
        [ObservableProperty]
        public Player player;

        [ObservableProperty]
        public Question question;
    }
}
