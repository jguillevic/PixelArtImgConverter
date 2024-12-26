using System.Drawing;

namespace PixelArtImgConverter
{
	public sealed class ImageProcessor
	{
		public Image ApplyPixelArtEffect(Image image, List<Color> palette, int basePixelSize)
		{
			// Calcule la taille du pixel visible en fonction de la résolution de l'image
			int pixelSize = CalculatePixelSize(image, basePixelSize);

			// Étape 1 : Réduction de la résolution
			Bitmap downscaled = DownscaleImage(image, pixelSize);

			// Étape 2 : Application de la palette (réduction des couleurs)
			Bitmap paletteImage = ApplyColorPalette(downscaled, palette) as Bitmap;

			// Étape 3 : Agrandissement pour obtenir des pixels visibles
			return UpscaleImage(paletteImage, pixelSize);
		}

		// Calcule la taille du pixel visible en fonction de la taille d'image
		private int CalculatePixelSize(Image image, int basePixelSize)
		{
			int maxWidth = 1920;  // Largeur maximale de l'image (par exemple, une largeur de 1920px)
			int maxHeight = 1080; // Hauteur maximale (par exemple, une hauteur de 1080px)

			// Calcule un facteur basé sur la résolution de l'image
			float widthRatio = (float)image.Width / maxWidth;
			float heightRatio = (float)image.Height / maxHeight;

			// Prend le plus grand facteur comme base pour l'ajustement
			float scaleFactor = Math.Max(widthRatio, heightRatio);

			// Ajuste la taille du pixel pour garder une taille logique tout en respectant la résolution de l'image
			return (int)(basePixelSize * scaleFactor);
		}

		private Bitmap DownscaleImage(Image image, int pixelSize)
		{
			int newWidth = image.Width / pixelSize;
			int newHeight = image.Height / pixelSize;

			Bitmap downscaled = new Bitmap(newWidth, newHeight);
			using (Graphics g = Graphics.FromImage(downscaled))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.DrawImage(image, 0, 0, newWidth, newHeight);
			}

			return downscaled;
		}

		private Bitmap UpscaleImage(Image image, int pixelSize)
		{
			int newWidth = image.Width * pixelSize;
			int newHeight = image.Height * pixelSize;

			Bitmap upscaled = new Bitmap(newWidth, newHeight);
			using (Graphics g = Graphics.FromImage(upscaled))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor; // Pas d'interpolation pour garder des pixels nets
				g.DrawImage(image, 0, 0, newWidth, newHeight);
			}

			return upscaled;
		}

		public Image ApplyColorPalette(Image image, List<Color> palette)
		{
			// Convertit l'image en un objet Bitmap pour permettre une modification pixel par pixel
			Bitmap bitmap = new Bitmap(image);
			Bitmap paletteImage = new Bitmap(image.Width, image.Height);

			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					Color pixelColor = bitmap.GetPixel(x, y);
					Color closestColor = GetClosestColor(pixelColor, palette);
					paletteImage.SetPixel(x, y, closestColor);
				}
			}

			return paletteImage;
		}

		private Color GetClosestColor(Color targetColor, List<Color> palette)
		{
			Color closestColor = palette[0];
			int closestDistance = int.MaxValue;

			foreach (Color paletteColor in palette)
			{
				int distance = GetColorDistance(targetColor, paletteColor);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestColor = paletteColor;
				}
			}

			return closestColor;
		}

		private int GetColorDistance(Color color1, Color color2)
		{
			return (int)Math.Sqrt(Math.Pow(color1.R - color2.R, 2) +
								  Math.Pow(color1.G - color2.G, 2) +
								  Math.Pow(color1.B - color2.B, 2));
		}

		// Méthode pour charger une image depuis un fichier
		public Image LoadImageFromFile(string filePath)
		{
			return Image.FromFile(filePath);
		}

		// Méthode pour enregistrer l'image modifiée dans un fichier
		public void SaveImageToFile(Image image, string filePath)
		{
			image.Save(filePath);
		}
	}
}
