using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TeamProject.Commends.ImageCommend;

public interface IGetSecretText
{
      string Execute(BitmapSource image);


}
public class GetSecretTextCommand : IGetSecretText
{
      private readonly string _signature = "STEG:"; 

    public string Execute(BitmapSource image)
    {
        if (image == null)
        {
            return string.Empty; 
        }

        if (image.Format != PixelFormats.Bgr32&&image.Format != PixelFormats.Bgra32)
        {
            throw new ArgumentException("Image must be in Bgr32 format.");
        }

        int stride = image.PixelWidth * 4; 
        byte[] pixelData = new byte[image.PixelHeight * stride];
        image.CopyPixels(pixelData, stride, 0);

        List<byte> messageBytes = new List<byte>();
        byte currentByte = 0;
        int bitCount = 0;

       
        for (int i = 0; i < pixelData.Length; i++)
        {
            if (i % 4 == 3) continue; 

            int lsb = pixelData[i] & 1; 
            currentByte = (byte)((currentByte << 1) | lsb); 
            bitCount++;

            if (bitCount % 8 == 0) 
            {
                messageBytes.Add(currentByte);
                if (currentByte == 0) break; 
                currentByte = 0;
            }
        }

      
        byte[] extractedBytes = messageBytes.ToArray();
        byte[] signatureBytes = Encoding.UTF8.GetBytes(_signature);

        if (extractedBytes.Length < signatureBytes.Length)
        {
            return string.Empty; 
        }

        for (int i = 0; i < signatureBytes.Length; i++)
        {
            if (extractedBytes[i] != signatureBytes[i])
            {
                return string.Empty; 
            }
        }
        
        byte[] messageOnlyBytes = new byte[extractedBytes.Length - signatureBytes.Length];
        Array.Copy(extractedBytes, signatureBytes.Length, messageOnlyBytes, 0, messageOnlyBytes.Length);

        return Encoding.UTF8.GetString(messageOnlyBytes);
    }

}
