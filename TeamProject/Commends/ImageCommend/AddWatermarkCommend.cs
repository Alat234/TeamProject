using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace TeamProject.Commends.ImageCommend;

public interface  IAddWatermarkCommend
{
     BitmapSource AddWatermark(BitmapSource image, string watermarkText, System.Windows.Media.Color color);
}
public class AddWatermarkCommend:IAddWatermarkCommend
{
     public BitmapSource AddWatermark(BitmapSource image, string watermarkText, System.Windows.Media.Color color)
     {
          if (image == null || string.IsNullOrEmpty(watermarkText)) return image;

          
          WriteableBitmap bitmap = new WriteableBitmap(image);

          FormattedText text = new FormattedText(
               watermarkText,
               System.Globalization.CultureInfo.CurrentCulture,
               FlowDirection.LeftToRight,
               new Typeface("Arial"),
               20,
               new SolidColorBrush(color),
               96);

          // Ініціалізуємо DrawingVisual для малювання
          DrawingVisual drawingVisual = new DrawingVisual();
          using (DrawingContext dc = drawingVisual.RenderOpen())
          {
               // Малюємо вихідне зображення як основу
               dc.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

               // Додаємо текст водяного знака в центрі
               double x = bitmap.PixelWidth / 2 - text.Width / 2;
               double y = bitmap.PixelHeight / 2 - text.Height / 2;
               dc.DrawText(text, new Point(x, y));
          }

          // Рендеримо результат у RenderTargetBitmap
          RenderTargetBitmap rtb = new RenderTargetBitmap(
               bitmap.PixelWidth,
               bitmap.PixelHeight,
               bitmap.DpiX,
               bitmap.DpiY,
               PixelFormats.Pbgra32);

          rtb.Render(drawingVisual);

          
          return new WriteableBitmap(rtb);
     }
}