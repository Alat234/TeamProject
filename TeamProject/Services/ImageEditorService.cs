
using System.Windows.Media.Imaging;
using TeamProject.Commends.ImageCommend;
using System.Windows.Media;
using TeamProject.Models.Filters;

namespace TeamProject.Services;

public interface  IImageEditorService
{
   
    BitmapSource  AddNoise(BitmapSource bitmapSource);
    string  GetSecretText(BitmapSource bitmapSource);
    BitmapSource AddSecretText(BitmapSource bitmapSource,string secretText);
    BitmapSource ApplyFilter(BitmapSource bitmapSource, BuildMethod method, ColorPalette colorPlate);


}
public class ImageEditorService: IImageEditorService
{
    
    private readonly IAddNoiseCommand _addNoiseCommand;
    private readonly IGetSecretText _getSecretText;
    private readonly IAddSecretTextCommand _addSecretTextCommand;
    private readonly IModifyLinesCommand _modifyLinesCommand;
    private readonly IModifyRainCommand _modifyHiperbolaCommand;
    private readonly IPixelationCommand _pixelationCommand;


    public ImageEditorService(IAddNoiseCommand addNoiseCommand,
        IGetSecretText getSecretText,IAddSecretTextCommand addSecretTextCommand, IModifyLinesCommand modifyLinesCommand
        ,IModifyRainCommand modifyRainCommand,IPixelationCommand pixelationCommand)
    {
        _pixelationCommand=pixelationCommand;
        _modifyHiperbolaCommand=modifyRainCommand;
        _modifyLinesCommand=modifyLinesCommand;
        _addSecretTextCommand=addSecretTextCommand;
        _getSecretText=getSecretText;
        _addNoiseCommand=addNoiseCommand;
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
        if(method == BuildMethod.Pixelation)
        {
            return _pixelationCommand.Execute(bitmapSource, colorPlate);
        }
        

        return bitmapSource;

    }
}