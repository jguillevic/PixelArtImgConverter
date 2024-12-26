// See https://aka.ms/new-console-template for more information
using PixelArtImgConverter;
using System.Drawing;


while (true)
{
	ImageProcessor processor = new ImageProcessor();

	string? pathToImage = string.Empty;

	while (string.IsNullOrEmpty(pathToImage) || !File.Exists(pathToImage))
	{
		Console.WriteLine("Chemin de l'image ?");
		pathToImage = Console.ReadLine();
	}

	// Charge l'image depuis un fichier
	Image image = processor.LoadImageFromFile(pathToImage);

	List<Color> palette = new List<Color>
	{
		Color.Black,         // Noir (pour les contours, ombres profondes)
		Color.White,         // Blanc (pour les reflets, détails clairs)
		Color.Gray,          // Gris (neutre, ombrages intermédiaires)
		Color.Red,           // Rouge vif (accents, objets vifs)
		Color.Green,         // Vert vif (nature, végétation)
		Color.Blue,          // Bleu vif (ciel, eau)
		Color.Cyan,          // Cyan (teinte froide, glace, détails technologiques)
		Color.Magenta,       // Magenta (fantaisie, mystère, détails)
		Color.Yellow,        // Jaune (soleil, or, accents lumineux)
		Color.Orange,        // Orange (feu, chaleur, objets énergiques)
		Color.Brown,         // Marron (terre, bois, teinte naturelle)
		Color.DarkGreen,     // Vert foncé (forêts, profondeur naturelle)
		Color.DarkBlue,      // Bleu foncé (nuits, ombres marines)
		Color.DarkRed,       // Rouge foncé (drame, profondeur)
		Color.LightGray,     // Gris clair (variations de lumière)
		Color.Purple         // Violet (fantastique, accents mystiques)
	};

	// Applique la palette de couleurs
	Image processedImage = processor.ApplyPixelArtEffect(image, palette, 5);

	var modifiedFileName = $"{DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff")}.png";
	// Sauvegarde l'image modifiée.
	processor.SaveImageToFile(processedImage, modifiedFileName);

	Console.WriteLine($"Transformation du fichier terminée. Sauvegarde sous le nom {modifiedFileName}");
}
