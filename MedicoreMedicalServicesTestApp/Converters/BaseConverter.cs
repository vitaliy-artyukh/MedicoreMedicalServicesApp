using System.Globalization;

namespace MedicoreMedicalServicesTestApp.Converters;

public abstract class BaseConverter<TFrom, TTo> : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Convert((TFrom?)value, targetType, parameter, culture);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => 
        ConvertBack((TTo?)value, targetType, parameter, culture);

    protected abstract TTo? Convert(TFrom? value, Type targetType, object? parameter, CultureInfo culture);

    protected virtual TFrom? ConvertBack(TTo? value, Type targetType, object? parameter, CultureInfo culture) =>
        default;
}