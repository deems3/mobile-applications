using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Collections.ObjectModel;
using TruthOrDrinkDemiBruls.Database;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.ViewModels;

namespace TruthOrDrinkDemiBruls.Views;

public partial class Lobby : ContentPage
{
    private readonly DatabaseContext _context;
    public ObservableCollection<Player> PlayersForDisplay { get; private set; }
    public Game Game { get; private set; }

    public LobbyViewModel viewModel { get; private set; }

    public Lobby(DatabaseContext context)
    {
        _context = context;

        InitializeComponent();

        // Generate lobby code
        var code = Guid.NewGuid().ToString("N")[..6];

        // Create new game
        Game = new Game
        {
            Code = code
        };

        //_context.Games.Add(_game);
        //_context.SaveChanges();

        QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();

        //ECCLevel = error correction level. Its possible to scan QR code even when its blurry.
        // Guid = unique string. ("n" specifies that no hyphens ("-") will be used
        // Guid is normally 32 chars long but the .. gives the opportunity to pick the first x chars
        QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.L);

        //Create the QRcode with pngBytes
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

        //Get byte data (bytes of the image)
        byte[] qrCodeBytes = qrCode.GetGraphic(20);

        QRImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));

        PlayersForDisplay = new ObservableCollection<Player>();

        viewModel = new LobbyViewModel(Game);
        BindingContext = viewModel;
    }

    private async void GoToGameOptions(object sender, EventArgs e)
    {
        if (Game.Players.Count == 0)
        {
            // TODO: show an erro message indicating that the game requires at least two players
            return;
        }

        _context.Games.Add(Game);
        _context.SaveChanges();

        await Shell.Current.GoToAsync(nameof(GameOptions), new Dictionary<string, object>
        {
            { "Game", Game },
        });
    }

    private async void ShowAddPlayerInput(object sender, EventArgs e)
    {
        var data = await DisplayPromptAsync("Speler toevoegen", "Speler naam", accept: "Toevoegen", cancel: "Annuleren");
        if (data is null)
        {
            return;
        }

        // Get the player from the database if it exists, otherwise create a new Player entity
        var player = await _context.Players.FirstOrDefaultAsync(x => x.Name.ToLower() == data.ToLower()) ?? new Player
        {
            Name = data
        };

        _context.Players.Update(player);

        // Add the player to the viewmodel to show it is selected
        viewModel.AddPlayer(player);
    }
}