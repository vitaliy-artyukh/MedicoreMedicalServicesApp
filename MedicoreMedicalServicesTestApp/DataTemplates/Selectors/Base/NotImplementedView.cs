namespace MedicoreMedicalServicesTestApp.DataTemplates.Selectors.Base;

public class NotImplementedView : ContentView
{
    public NotImplementedView()
    {
        Content = new Label
        {
            Text = "View under construction",
            TextColor = Colors.Gray,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            Padding = 12
        };
    }
}