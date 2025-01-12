using CommunityToolkit.Mvvm.ComponentModel;
using TruthOrDrinkDemiBruls.Database;
using TruthOrDrinkDemiBruls.Enums;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.Service
{
    public class GameService(DatabaseContext context) : ObservableObject
    {
        public Game? Game { get; private set; }
        public int QuestionAmount { get; set; }
        public List<Theme> Themes { get; set; } = [];
        public QuestionKind QuestionKind { get; set; }
        public QuestionIntensity QuestionIntensity { get; set; }

        public void InitializeGame(string code)
        {
            Game = new Game()
            {
                Code = code
            };
        }

        public void StartGame()
        {
            if (Game == null) {
                return;
            }
            context.Games.Add(Game);
            context.SaveChanges();
        }

        public async void AddPlayer(Player player)
        {
            if (Game is null)
            {
                return;
            }

            var existingPlayer = Game.Players.FirstOrDefault(x => x.Name.ToLower() == player.Name.ToLower());
            if (existingPlayer != null)
            {
                Game.Players.Remove(existingPlayer);
            }

            Game.Players.Add(player);
            await context.SaveChangesAsync();
        }
    }
}
