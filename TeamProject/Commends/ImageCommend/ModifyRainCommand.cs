using System.Windows.Media;
using System.Windows.Media.Imaging;
using TeamProject.Models.Filters;

namespace TeamProject.Commends.ImageCommend;


    public interface IModifyRainCommand
    {
        BitmapSource Execute(BitmapSource bitmapSource, ColorPalette palette);
    }

    public class ModifyRainCommand : IModifyRainCommand
    {
        private Random _random = new Random(); 
        public BitmapSource Execute(BitmapSource bitmapSource, ColorPalette palette)
        {
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * 4;
            byte[] pixelData = new byte[height * stride];
            bitmapSource.CopyPixels(pixelData, stride, 0);

            Random random = new Random();

           
            int dropCount = random.Next(100, 200);

            for (int i = 0; i < dropCount; i++)
            {
               
                double centerX = random.Next(width); 
                double centerY = random.Next(height);
                double a = random.Next(1, 3); 
                double b = random.Next(6, 12); 

               
                DrawRainDrop(pixelData, width, height, stride, centerX, centerY, a, b, palette);
            }

            return BitmapFrame.Create(width, height, bitmapSource.DpiX, bitmapSource.DpiY, PixelFormats.Bgra32, null,
                pixelData, stride);
        }

        private void DrawRainDrop(byte[] pixelData, int width, int height, int stride, double centerX, double centerY, double a, double b, ColorPalette palette)
        {
            
            int rangeX = (int)(a * 2); 
            int rangeY = (int)(b * 2); 

            for (int dy = -rangeY; dy <= rangeY; dy++)
            {
                for (int dx = -rangeX; dx <= rangeX; dx++)
                {
                    double x = dx + centerX;
                    double y = dy + centerY;

                    
                    double xNorm = dx;
                    double yNorm = dy;
                    double ellipseValue = (xNorm * xNorm) / (a * a) + (yNorm * yNorm) / (b * b);

                 
                    if (ellipseValue <= 1.2) 
                    {
                        int newX = (int)x;
                        int newY = (int)y;

                        if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                        {
                            int index = (newY * width + newX) * 4;

                          
                            byte lsbB = (byte)(pixelData[index] & 1);
                            byte lsbG = (byte)(pixelData[index + 1] & 1);
                            byte lsbR = (byte)(pixelData[index + 2] & 1);

                            byte red, green, blue;
                            double intensityFactor = 1.0 - Math.Pow(ellipseValue, 2) / 2.25; 
                            intensityFactor = Math.Max(0, Math.Min(1, intensityFactor));
                            double opacity = Math.Max(0, 1 - (ellipseValue - 1) * 5); 
                            switch (palette)
                            {
                             case ColorPalette.RedBlue:
                            if (_random.Next(2) == 0) 
                            {
                                red = (byte)(200 * intensityFactor); 
                                green = (byte)(0 * intensityFactor);
                                blue = (byte)(0 * intensityFactor);
                            }
                            else 
                            {
                                red = (byte)(0 * intensityFactor);
                                green = (byte)(0 * intensityFactor);
                                blue = (byte)(200 * intensityFactor); 
                            }
                            break;
                        case ColorPalette.GreenYellow:
                            if (_random.Next(2) == 0) 
                            {
                                red = (byte)(0 * intensityFactor);    
                                green = (byte)(200 * intensityFactor); 
                                blue = (byte)(0 * intensityFactor);    
                            }
                            else 
                            {
                                red = (byte)(200 * intensityFactor);  
                                green = (byte)(200 * intensityFactor); 
                                blue = (byte)(0 * intensityFactor);   
                            }
                            break;
                        case ColorPalette.Monochrome:
                            if (_random.Next(2) == 0) 
                            {
                                red = (byte)(200 * intensityFactor);
                                green = (byte)(200 * intensityFactor);
                                blue = (byte)(200 * intensityFactor);
                            }
                            else 
                            {
                                red = (byte)(80 * intensityFactor);
                                green = (byte)(80 * intensityFactor);
                                blue = (byte)(80 * intensityFactor);
                            }
                            break;
                        default:
                            red = (byte)(150 * intensityFactor);
                            green = (byte)(200 * intensityFactor);
                            blue = (byte)(250 * intensityFactor);
                            break;
                            }
                            
                            pixelData[index] = (byte)((blue & 0xFE) | lsbB); // B
                            pixelData[index + 1] = (byte)((green & 0xFE) | lsbG); // G
                            pixelData[index + 2] = (byte)((red & 0xFE) | lsbR); // R
                            pixelData[index + 3] = 255; // A
                        }
                    }
                }
            }
        }
       
        
        
           

    }
    
      
    
   