using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MedicoreMedicalServicesTestApp.DataTemplates.Selectors.Base;

namespace MedicoreMedicalServicesTestApp.DataTemplates.Selectors;

[ContentProperty(nameof(Templates))]
[AcceptEmptyServiceProvider]
public class XamlTemplateSelector : BaseTemplateSelector
{
    private bool _isSubscribed = true;

    public ObservableCollection<XamlTemplateBase> Templates { get; }

    public DataTemplate? DefaultTemplate { get; set; }

    protected override Func<object?>? NotImplementedTemplateCreator
    {
        get => DefaultTemplate != null ? () => DefaultTemplate.CreateContent() : null;
    }

    public XamlTemplateSelector()
    {
        Templates = new ObservableCollection<XamlTemplateBase>();
        Templates.CollectionChanged += TemplatesOnCollectionChanged;
    }

    private void TemplatesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Add)
        {
            return;
        }

        if (e.NewItems == null)
        {
            return;
        }

        foreach (XamlTemplateBase template in e.NewItems)
        {
            TemplatesDictionaryByViewModel.Add(template.DataType, template.DataTemplate);
        }
    }

    protected override DataTemplate OnSelectTemplate(object? item, BindableObject container)
    {
        if (_isSubscribed)
        {
            _isSubscribed = false;
            Templates.CollectionChanged -= TemplatesOnCollectionChanged;
        }

        return base.OnSelectTemplate(item, container);
    }


    protected override void InitializeDataTemplates()
    {
        //ignore
    }
}

public abstract class XamlTemplateBase
{
    protected XamlTemplateBase(Type dataType)
    {
        DataType = dataType;
    }

    public Type DataType { get; }
    public DataTemplate DataTemplate { get; set; } = null!;
}

[ContentProperty(nameof(DataTemplate))]
public class XamlTemplate<T> : XamlTemplateBase
{
    public XamlTemplate() : base(typeof(T))
    {
    }
}