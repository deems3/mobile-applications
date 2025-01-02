using System.Collections.ObjectModel;
using System.Windows.Input;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.ViewModels;

public class LobbyViewModel
{
    public ObservableCollection<Player> PlayersForDisplay { get; private set; } = [];
    public Game Game { get; private set; }

    public ICommand DeletePlayerCommand { get; private set; }

    public LobbyViewModel(Game game)
    {
        Game = game;
        DeletePlayerCommand = new Command<string>(DeletePlayerFromGame);
    }

    public void AddPlayer(Player player)
    {
        Game.Players.Add(player);

        if (!PlayersForDisplay.Contains(player))
        {
            PlayersForDisplay.Add(player);
        }
    }

    private void DeletePlayerFromGame(string name)
    {
        var player = Game.Players.First(x => x.Name == name);
        Game.Players.Remove(player);

        var viewPlayer = PlayersForDisplay.First(x => x.Name == name);
        PlayersForDisplay.Remove(viewPlayer);
    }
}
