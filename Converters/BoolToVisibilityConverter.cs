using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KeySoundChan.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            // Inverse 파라미터가 있으면 반전
            var inverse = parameter is string s && s == "Inverse";
            if (inverse) boolValue = !boolValue;
            
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            var result = visibility == Visibility.Visible;
            var inverse = parameter is string s && s == "Inverse";
            return inverse ? !result : result;
        }
        return false;
    }
}

public class PercentToWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // 슬라이더 값을 퍼센트 너비로 변환 (대략적인 값)
        if (value is double doubleValue)
        {
            return doubleValue * 3.4; // 슬라이더 트랙 너비에 맞춤
        }
        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class PercentToOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // 0~100 정수를 0.0~1.0 double로 변환
        if (value is int intValue)
        {
            return intValue / 100.0;
        }
        return 1.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double doubleValue)
        {
            return (int)(doubleValue * 100);
        }
        return 100;
    }
}

