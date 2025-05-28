using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TeamProject.Commends.ImageCommend;

public interface IAddNoiseCommand
{
    BitmapSource AddNoise (BitmapSource bitmapSource);
    
}
public class AddNoiseCommand:IAddNoiseCommand
{
    private readonly Random _random = new Random();
    public BitmapSource AddNoise(BitmapSource bitmapSource)
    {
        if (bitmapSource == null) return null;

        var width = bitmapSource.PixelWidth;
        var height = bitmapSource.PixelHeight;
        PixelFormat imageFormat = bitmapSource.Format;
        var stride = (width * imageFormat.BitsPerPixel + 7) / 8;

        var pixels = new byte[height * stride];
        bitmapSource.CopyPixels(pixels, stride, 0);

        if (imageFormat == PixelFormats.Bgr32 || imageFormat == PixelFormats.Pbgra32|| imageFormat == PixelFormats.Bgra32)
        {
            for (var i = 0; i < pixels.Length; i += 4)
            {
                var noiseBlue = _random.Next(-128, 128);
                var noiseGreen = _random.Next(-128, 128);
                var noiseRed = _random.Next(-128, 128);

                var noisyBlue = pixels[i] + noiseBlue;
                var noisyGreen = pixels[i + 1] + noiseGreen;
                var noisyRed = pixels[i + 2] + noiseRed;

                pixels[i] = (byte)Math.Max(0, Math.Min(255, noisyBlue));     
                pixels[i + 1] = (byte)Math.Max(0, Math.Min(255, noisyGreen)); 
                pixels[i + 2] = (byte)Math.Max(0, Math.Min(255, noisyRed));   
                pixels[i + 3] = 255;   
            }
        }

        WriteableBitmap newBitmap = new WriteableBitmap(
            width, height, bitmapSource.DpiX, bitmapSource.DpiY, imageFormat, null);
        
        newBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);

        return newBitmap;
    }
}