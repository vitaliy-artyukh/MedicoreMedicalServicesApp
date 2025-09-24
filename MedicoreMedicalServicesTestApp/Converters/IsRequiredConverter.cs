using System.Globalization;

namespace MedicoreMedicalServicesTestApp.Converters;

public class IsRequiredConverter : BaseConverter<bool, string>
{
    protected override string? Convert(bool value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value ? "* " : null;
    }
}