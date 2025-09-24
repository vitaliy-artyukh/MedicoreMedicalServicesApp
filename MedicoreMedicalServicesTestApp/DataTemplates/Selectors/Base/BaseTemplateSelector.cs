namespace MedicoreMedicalServicesTestApp.DataTemplates.Selectors.Base;

public abstract class BaseTemplateSelector : DataTemplateSelector, IMarkupExtension
{
    private DataTemplate _notImplementedTemplate = null!;
    private DataTemplate _emptyTemplate = null!;

    private bool _isInitialized;

    protected virtual Type NotImplementedTemplateType => typeof(NotImplementedView);
    protected virtual Func<object?>? NotImplementedTemplateCreator => null;

    protected virtual Type EmptyTemplateType => typeof(EmptyView);
    protected virtual Func<object?>? EmptyTemplateCreator => null;

    protected virtual Func<object, DataTemplate?>? CustomTemplateValidator => null;

    protected Dictionary<Type, DataTemplate> TemplatesDictionaryByViewModel { get; }
    protected Dictionary<Type, DataTemplate> TemplatesDictionaryByView { get; }

    protected BaseTemplateSelector()
    {
        TemplatesDictionaryByViewModel = new Dictionary<Type, DataTemplate>();
        TemplatesDictionaryByView = new Dictionary<Type, DataTemplate>();
    }

    protected abstract void InitializeDataTemplates();

    protected DataTemplate AddDataTemplate<TViewModel, TView>()
        where TViewModel : class
        where TView : Element
    {
        return AddDataTemplate(typeof(TViewModel), typeof(TView));
    }

    protected DataTemplate AddDataTemplate(Type viewModelType, Type viewType)
    {
        if (!TemplatesDictionaryByView.TryGetValue(viewType, out var template))
        {
            template = new DataTemplate(viewType);
            TemplatesDictionaryByView[viewType] = template;
        }

        if (CustomTemplateValidator != null)
            return template;

        TemplatesDictionaryByViewModel[viewModelType] = template;
        return template;
    }

    protected override DataTemplate OnSelectTemplate(object? item, BindableObject container)
    {
        if (!_isInitialized)
        {
            _isInitialized = true;

            _notImplementedTemplate = NotImplementedTemplateCreator != null
                ? new DataTemplate(NotImplementedTemplateCreator)
                : new DataTemplate(NotImplementedTemplateType);

            _emptyTemplate = EmptyTemplateCreator != null
                ? new DataTemplate(EmptyTemplateCreator)
                : new DataTemplate(EmptyTemplateType);

            InitializeDataTemplates();
        }

        if (item == null)
        {
            return _notImplementedTemplate;
        }

        var itemType = item.GetType();
        
        itemType = Nullable.GetUnderlyingType(itemType) ?? itemType;

        if (CustomTemplateValidator == null)
        {
            return TemplatesDictionaryByViewModel.GetValueOrDefault(itemType, _notImplementedTemplate);
        }

        var res = CustomTemplateValidator(item);
        return res ?? _notImplementedTemplate;
    }

    public virtual object ProvideValue(IServiceProvider serviceProvider) =>
        this;
}