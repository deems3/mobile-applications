using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Windows.Input;
using TruthOrDrinkDemiBruls.Database;
using TruthOrDrinkDemiBruls.Models;
using TruthOrDrinkDemiBruls.ViewModels;

namespace TruthOrDrinkDemiBruls.Views;

public partial class Lobby : ContentPage
{
    private readonly DatabaseContext _context;
    public Game Game { get; private set; }

    public LobbyViewModel viewModel { get; private set; }

    public ICommand UpdatePlayerImageCommand { get; private set; }

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

        viewModel = new LobbyViewModel(Game);

        UpdatePlayerImageCommand = new Command<string>(UpdatePlayerImage);

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
        _context.SaveChanges();

        // Add the player to the viewmodel to show it is selected
        viewModel.AddPlayer(player);
    }

    private async void UpdatePlayerImage(string name)
    {
        var result = await DisplayActionSheet("Kies een optie om de afbeelding te wijzigen", "Annuleren", null, "Camera", "Gallerij");

        FileResult? photo = null;

        if (result == "Camera")
        {
            photo = await MediaPicker.CapturePhotoAsync();
        }

        if (result == "Gallerij")
        {
            photo = await MediaPicker.PickPhotoAsync();
        }

        if (photo is null)
        {
            return;
        }

        using var stream = await photo.OpenReadAsync();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var imageBytes = memoryStream.ToArray();
        var base64String = Convert.ToBase64String(imageBytes);

        var player = await _context.Players.AsNoTracking().FirstAsync(x => x.Name.ToLower() == name.ToLower());
        player.ImageContents = base64String;

        _context.Players.Update(player);
        await _context.SaveChangesAsync();

        var freshPlayer = await _context.Players.AsNoTracking().FirstAsync(x => x.Name == name);

        viewModel.UpdatePlayerImage(freshPlayer);
    }
}