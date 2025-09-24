namespace MedicoreMedicalServicesTestApp.Controls;

public class DataTemplateView : ContentView
{
    public static readonly BindableProperty TemplateProperty = BindableProperty.Create(
        nameof(Template),
        typeof(DataTemplate),
        typeof(DataTemplateView),
        propertyChanged: OnTemplatePropertyChanged
    );

    public DataTemplate? Template
    {
        get => (DataTemplate?)GetValue(TemplateProperty);
        set => SetValue(TemplateProperty, value);
    }

    private static void OnTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is DataTemplateView parent)
            || !(newValue is DataTemplate newV))
            return;

        parent.RedrawView();
    }

    private void RedrawView()
    {
        if (Template is null || BindingContext is null)
            return;

        var template = Template is DataTemplateSelector selector ? selector.SelectTemplate(BindingContext, null) : Template;
        
        Content = template.CreateContent() as View;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        
        RedrawView();
    }
}