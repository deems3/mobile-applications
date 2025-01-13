using CommunityToolkit.Mvvm.ComponentModel;
using System.Data.Entity;
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
        public Player? PlayerToAnswer { get; private set; }
        public GameQuestion? QuestionToAnswer { get; private set; }

        public void InitializeGame(string code)
        {
            Game = new Game()
            {
                Code = code
            };
        }

        public void StartGame()
        {
            if (Game == null)
            {
                return;
            }
            context.Games.Add(Game);

            // Get the theme Ids from the Themes property
            var themeIds = Themes.Select(t => t.Id).ToList();

            // Get random questions from the database where the questions are linked to the specified themes
            var questions = context.Questions
                    .Where(q => q.Themes.Any(t => themeIds.Contains(t.Id))) // Filter questions that are related to the theme
                    .ToList();

            var randomQuestions = questions
                .OrderBy(q => Guid.NewGuid()) // randomize questions
                .Take(QuestionAmount) // take x amount of questions
                .ToList();

            foreach (var question in randomQuestions)
            {
                Game.GameQuestions.Add(new GameQuestion
                {
                    GameId = Game.Id,
                    QuestionId = question.Id
                });
            }

            context.SaveChanges();
            SetPlayerToAnswer();
            SetNextQuestionToAnswer();
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

        public void SetNextQuestionToAnswer()
        {
            QuestionToAnswer = Game!.GameQuestions.Where(q => q.Player == null).FirstOrDefault();
        }

        public void SetPlayerToAnswer()
        {
            var random = new Random();
            int randomIndex = random.Next(Game!.Players.Count);
            var player = Game.Players.ElementAt(randomIndex);

            if (player.Id == PlayerToAnswer?.Id)
            {
                SetPlayerToAnswer();
            }

            PlayerToAnswer = Game.Players.ElementAt(randomIndex);
        }

        public void AnswerQuestion()
        {
            QuestionToAnswer!.PlayerId = PlayerToAnswer!.Id;
            context.GameQuestions.Update(QuestionToAnswer);
            context.SaveChanges();
        }
    }
}
