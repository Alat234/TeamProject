using System.Windows.Media;
using System.Windows.Media.Imaging;
using TeamProject.Models.Filters;

namespace TeamProject.Commends.ImageCommend;
public interface  IModifyLinesCommand
{
    BitmapSource Execute(BitmapSource bitmapSource, ColorPalette palette);

}

public class ModifyLinesCommand : IModifyLinesCommand
{
    public BitmapSource Execute(BitmapSource bitmapSource, ColorPalette palette)
    {
        

        int width = bitmapSource.PixelWidth;
        int height = bitmapSource.PixelHeight;
        int stride = width * 4;
        byte[] pixelData = new byte[height * stride];
        bitmapSource.CopyPixels(pixelData, stride, 0);

        Random random = new Random();

        for (int y = 0; y < height; y++)
        {
            int lineWidth = random.Next(5, 15);
            bool isLine = (y % (lineWidth * 2)) < lineWidth;

            for (int x = 0; x < width; x++)
            {
                int index = (y * width + x) * 4;

                if (isLine)
                {
                    // Seve LSB 
                    byte lsbB = (byte)(pixelData[index] & 1);
                    byte lsbG = (byte)(pixelData[index + 1] & 1);
                    byte lsbR = (byte)(pixelData[index + 2] & 1);

                    byte r, g, b;
                    switch (palette)
                    {
                        case ColorPalette.RedBlue:
                            r = (byte)(255 * (x % 2));
                            b = (byte)(255 * ((x + 1) % 2));
                            g = 0;
                            break;
                        case ColorPalette.GreenYellow:
                            g = (byte)(255 * (x % 2));
                            r = (byte)(255 * (x % 2));
                            b = 0;
                            break;
                        case ColorPalette.Monochrome:
                            byte gray = (byte)(128 + 127 * Math.Sin(y * 0.1));
                            r = g = b = gray;
                            break;
                        default:
                            throw new NotSupportedException("Unsupported color palette.");
                    }

                    // Recover LSB 
                    pixelData[index] = (byte)((b & 0xFE) | lsbB); // B
                    pixelData[index + 1] = (byte)((g & 0xFE) | lsbG); // G
                    pixelData[index + 2] = (byte)((r & 0xFE) | lsbR); // R
                    pixelData[index + 3] = 255; // A
                }
            }
        }

        return BitmapFrame.Create(width, height, bitmapSource.DpiX, bitmapSource.DpiY, PixelFormats.Bgra32, null,
            pixelData, stride);
    }
}