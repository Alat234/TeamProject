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
        public BitmapSource Execute(BitmapSource bitmapSource, ColorPalette palette)
        {
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * 4;
            byte[] pixelData = new byte[height * stride];
            bitmapSource.CopyPixels(pixelData, stride, 0);

            Random random = new Random();

            // Кількість крапель
            int dropCount = random.Next(100, 200); // 100–200 крапель для ефекту дощу

            for (int i = 0; i < dropCount; i++)
            {
                // Випадкові параметри для кожної краплі
                double centerX = random.Next(width); // Випадкове положення по X
                double centerY = random.Next(height); // Випадкове положення по Y
                double a = random.Next(1, 3); // Ширина еліпса (менша)
                double b = random.Next(6, 12); // Висота еліпса (більша для витягнутої форми)

                // Малюємо краплю як еліпс
                DrawRainDrop(pixelData, width, height, stride, centerX, centerY, a, b, palette);
            }

            return BitmapFrame.Create(width, height, bitmapSource.DpiX, bitmapSource.DpiY, PixelFormats.Bgra32, null,
                pixelData, stride);
        }

        private void DrawRainDrop(byte[] pixelData, int width, int height, int stride, double centerX, double centerY, double a, double b, ColorPalette palette)
        {
            // Ітеруємо по достатньо великій області, щоб охопити весь еліпс
            int rangeX = (int)(a * 2); // Область ітерації по X
            int rangeY = (int)(b * 2); // Область ітерації по Y

            for (int dy = -rangeY; dy <= rangeY; dy++)
            {
                for (int dx = -rangeX; dx <= rangeX; dx++)
                {
                    double x = dx + centerX;
                    double y = dy + centerY;

                    // Рівняння еліпса: (x'^2 / a^2) + (y'^2 / b^2)
                    double xNorm = dx;
                    double yNorm = dy;
                    double ellipseValue = (xNorm * xNorm) / (a * a) + (yNorm * yNorm) / (b * b);

                    // Додаємо згладжування: якщо значення близько до 1, зменшуємо інтенсивність кольору
                    if (ellipseValue <= 1.2) // Трохи ширше для згладжування
                    {
                        int newX = (int)x;
                        int newY = (int)y;

                        if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                        {
                            int index = (newY * width + newX) * 4;

                            // Зберігаємо LSB для секретного тексту
                            byte lsbB = (byte)(pixelData[index] & 1);
                            byte lsbG = (byte)(pixelData[index + 1] & 1);
                            byte lsbR = (byte)(pixelData[index + 2] & 1);

                            byte red, green, blue;
                            double opacity = Math.Max(0, 1 - (ellipseValue - 1) * 5); // Згладжування країв
                            switch (palette)
                            {
                                case ColorPalette.RedBlue:
                                    red = 0; // Блакитний
                                    green = 0;
                                    blue = (byte)(200 * opacity); // Зменшуємо інтенсивність на краях
                                    break;
                                case ColorPalette.GreenYellow:
                                    red = 0; // Зелений
                                    green = (byte)(150 * opacity);
                                    blue = 0;
                                    break;
                                case ColorPalette.Monochrome:
                                    red = green = blue = (byte)(128 * opacity); // Сірий
                                    break;
                                default:
                                    throw new NotSupportedException("Unsupported color palette.");
                            }

                            // Відновлюємо LSB
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
    
      
    
   