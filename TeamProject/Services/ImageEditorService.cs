
using System.Windows.Media.Imaging;
using TeamProject.Commends.ImageCommend;
using System.Windows.Media;
using TeamProject.Models.Filters;

namespace TeamProject.Services;

public interface  IImageEditorService
{
    BitmapSource AddWatermark(BitmapSource image, string watermarkText, Color color);
    BitmapSource  AddNoise(BitmapSource bitmapSource);
    string  GetSecretText(BitmapSource bitmapSource);
    BitmapSource AddSecretText(BitmapSource bitmapSource,string secretText);
    BitmapSource ApplyFilter(BitmapSource bitmapSource, BuildMethod method, ColorPalette colorPlate);


}
public class ImageEditorService: IImageEditorService
{
    private readonly IAddWatermarkCommend _addWatermarkCommend;
    private readonly IAddNoiseCommand _addNoiseCommand;
    private readonly IGetSecretText _getSecretText;
    private readonly IAddSecretTextCommand _addSecretTextCommand;
    private readonly IModifyLinesCommand _modifyLinesCommand;
    private readonly IModifyRainCommand _modifyHiperbolaCommand;


    public ImageEditorService(IAddWatermarkCommend addWatermarkCommend,IAddNoiseCommand addNoiseCommand,
        IGetSecretText getSecretText,IAddSecretTextCommand addSecretTextCommand, IModifyLinesCommand modifyLinesCommand
        ,IModifyRainCommand modifyRainCommand)
    {
        _modifyHiperbolaCommand=modifyRainCommand;
        _modifyLinesCommand=modifyLinesCommand;
        _addSecretTextCommand=addSecretTextCommand;
        _getSecretText=getSecretText;
        _addWatermarkCommend=addWatermarkCommend;
        _addNoiseCommand=addNoiseCommand;
    }
    public BitmapSource AddWatermark(BitmapSource image, string watermarkText, Color color)
    {
       return _addWatermarkCommend.AddWatermark(image, watermarkText, color);
    }

    public BitmapSource AddNoise(BitmapSource bitmapSource)
    {
        return  _addNoiseCommand.AddNoise(bitmapSource);
    }

    public string GetSecretText(BitmapSource bitmapSource)
    {
        return _getSecretText.Execute(bitmapSource);
        
    }

    public BitmapSource AddSecretText(BitmapSource bitmapSource, string secretText)
    {
        return  _addSecretTextCommand.Execute(bitmapSource, secretText);
        
    }

    public BitmapSource ApplyFilter(BitmapSource bitmapSource, BuildMethod method, ColorPalette colorPlate)
    {
        if (method == BuildMethod.Lines)
        {
            return  _modifyLinesCommand.Execute(bitmapSource, colorPlate);
        }
        if(method == BuildMethod.Rain)
        {
            return _modifyHiperbolaCommand.Execute(bitmapSource, colorPlate);
        }
        

        return bitmapSource;

    }
}