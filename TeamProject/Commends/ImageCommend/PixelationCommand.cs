using System.Windows.Media;
using System.Windows.Media.Imaging;
using TeamProject.Models.Filters;

namespace TeamProject.Commends.ImageCommend;

public interface IPixelationCommand
{
    BitmapSource Execute(BitmapSource bitmapSource, ColorPalette palette, int pixelSize = 10);
}

public class PixelationCommand : IPixelationCommand
{
    /// <summary>
    /// Застосовує ефект пікселізації до зображення, зберігаючи молодші біти для потенційного шифрування.
    /// </summary>
    /// <param name="bitmapSource">Вихідне зображення.</param>
    /// <param name="palette">Палітра кольорів (не використовується напряму для пікселізації, але може бути адаптована).</param>
    /// <param name="pixelSize">Розмір квадрата пікселізації (наприклад, 10 для блоків 10x10 пікселів).</param>
    /// <returns>Зображення із застосованим ефектом пікселізації.</returns>
    public BitmapSource Execute(BitmapSource bitmapSource, ColorPalette palette, int pixelSize = 10)
    {
        if (bitmapSource == null) throw new ArgumentNullException(nameof(bitmapSource));
        if (pixelSize <= 0) throw new ArgumentOutOfRangeException(nameof(pixelSize), "Pixel size must be greater than 0.");

        int width = bitmapSource.PixelWidth;
        int height = bitmapSource.PixelHeight;
        int stride = width * 4; // Припускаємо формат Bgra32 (4 байти на піксель: Blue, Green, Red, Alpha)
        byte[] pixelData = new byte[height * stride];
        bitmapSource.CopyPixels(pixelData, stride, 0);

        for (int y = 0; y < height; y += pixelSize)
        {
            for (int x = 0; x < width; x += pixelSize)
            {
                // Обчислюємо середній колір для поточного блоку
                int sumB = 0;
                int sumG = 0;
                int sumR = 0;
                int count = 0;

                for (int subY = y; subY < Math.Min(y + pixelSize, height); subY++)
                {
                    for (int subX = x; subX < Math.Min(x + pixelSize, width); subX++)
                    {
                        int index = (subY * width + subX) * 4;
                        sumB += pixelData[index];
                        sumG += pixelData[index + 1];
                        sumR += pixelData[index + 2];
                        count++;
                    }
                }

                byte avgB = (byte)(sumB / count);
                byte avgG = (byte)(sumG / count);
                byte avgR = (byte)(sumR / count);

                // Тепер застосовуємо цей середній колір до всього блоку, зберігаючи LSB
                for (int subY = y; subY < Math.Min(y + pixelSize, height); subY++)
                {
                    for (int subX = x; subX < Math.Min(x + pixelSize, width); subX++)
                    {
                        int index = (subY * width + subX) * 4;

                        // Зберігаємо LSB оригінального пікселя перед модифікацією
                        byte lsbB = (byte)(pixelData[index] & 1);
                        byte lsbG = (byte)(pixelData[index + 1] & 1);
                        byte lsbR = (byte)(pixelData[index + 2] & 1);

                        // Призначаємо середній колір блоку
                        pixelData[index] = (byte)((avgB & 0xFE) | lsbB);     // B
                        pixelData[index + 1] = (byte)((avgG & 0xFE) | lsbG); // G
                        pixelData[index + 2] = (byte)((avgR & 0xFE) | lsbR); // R
                        pixelData[index + 3] = 255; // Alpha
                    }
                }
            }
        }

        return BitmapFrame.Create(width, height, bitmapSource.DpiX, bitmapSource.DpiY, PixelFormats.Bgra32, null,
            pixelData, stride);
    }
}