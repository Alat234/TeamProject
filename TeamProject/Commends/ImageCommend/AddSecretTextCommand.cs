using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TeamProject.Commends.ImageCommend;

public interface IAddSecretTextCommand
{
    public BitmapSource Execute(BitmapSource image, string message);
    
}
public class AddSecretTextCommand: IAddSecretTextCommand
{
    private readonly string _signature = "STEG:";
    public BitmapSource Execute(BitmapSource image, string message)
    {
        if (image == null || string.IsNullOrEmpty(message))
        {
            return image; 
        }

       
        if (image.Format != PixelFormats.Bgr32 && image.Format != PixelFormats.Bgra32)
        {
            throw new ArgumentException("Image must be in Bgr32 or Bgra32 format.");
        }

        int stride = image.PixelWidth * 4; 
        byte[] pixelData = new byte[image.PixelHeight * stride];
        image.CopyPixels(pixelData, stride, 0);
        
        string fullMessage = _signature + message + "\0";
        byte[] messageBytes = Encoding.UTF8.GetBytes(fullMessage);

        int messageIndex = 0;
        int bitIndex = 0;

        for (int i = 0; i < pixelData.Length && messageIndex < messageBytes.Length; i++)
        {
            
            if (i % 4 == 3) continue;

            if (bitIndex < 8)
            {
                int bit = (messageBytes[messageIndex] >> (7 - bitIndex)) & 1;
                pixelData[i] = (byte)((pixelData[i] & 0xFE) | bit); // Замінюємо LSB
                bitIndex++;
            }
            else
            {
                messageIndex++;
                bitIndex = 0;
                i--; 
                continue;
            }
        }

        var bitmapFrame = BitmapFrame.Create(image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY, image.Format, image.Palette, pixelData, stride);
        return bitmapFrame;
        
       
    }
}