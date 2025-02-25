﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using TruthOrDrinkDemiBruls.Models;

namespace TruthOrDrinkDemiBruls.ViewModels;

public class LobbyViewModel
{
    public ObservableCollection<Player> PlayersForDisplay { get; private set; } = [];
    public Game Game { get; private set; }

    public ICommand DeletePlayerCommand { get; private set; }
    public ICommand UpdatePlayerImageCommand { get; private set; }

    public LobbyViewModel(Game game)
    {
        Game = game;
        // Command allows an action to be called with parameters (e.g. the player's name)
        DeletePlayerCommand = new Command<string>(DeletePlayerFromGame);
        UpdatePlayerImageCommand = new Command<Player>(UpdatePlayerImage);
    }

    public void AddPlayer(Player player)
    {
        // Add the player to the Game
        Game.Players.Add(player);

        // If the player is not in the selected players collection yet, add it as well
        if (!PlayersForDisplay.Contains(player))
        {
            PlayersForDisplay.Add(player);
        }
    }

    public void UpdatePlayerImage(Player player)
    {
        var gamePlayer = Game.Players.First(x => x.Id == player.Id);
        Game.Players.Remove(gamePlayer);
        Game.Players.Add(player);

        var displayPlayer = PlayersForDisplay.First(x => x.Id == player.Id);

        PlayersForDisplay.Remove(displayPlayer);
        PlayersForDisplay.Add(gamePlayer);
    }

    private void DeletePlayerFromGame(string name)
    {
        // Delete the player from the game
        var player = Game.Players.First(x => x.Name == name);
        Game.Players.Remove(player);

        // Delete the player from the collection
        var viewPlayer = PlayersForDisplay.First(x => x.Name == name);
        PlayersForDisplay.Remove(viewPlayer);
    }
}
