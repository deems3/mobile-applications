using QRCoder;

namespace TruthOrDrinkDemiBruls.Views;

public partial class Lobby : ContentPage
{
	public Lobby()
	{
		InitializeComponent();

        QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();

        //ECCLevel = error correction level. Its possible to scan QR code even when its blurry.
        // Guid = unique string. ("n" specifies that no hyphens ("-") will be used
        // Guid is normally 32 chars long but the .. gives the opportunity to pick the first x chars
        QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(Guid.NewGuid().ToString("N")[..6], QRCodeGenerator.ECCLevel.L);

        //Create the QRcode with pngBytes
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

        //Get byte data (bytes of the image)
        byte[] qrCodeBytes = qrCode.GetGraphic(20);

        QRImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
    }
}