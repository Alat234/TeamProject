using System.Globalization;
using System.Windows.Data;
using AutoMapper;

namespace TeamProject.Mappers;

public class InverseOpacityConverter: IValueConverter
{
    public  object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is double blendValue)
        {
            return 1.0 - blendValue; 
        }
        return 1.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}