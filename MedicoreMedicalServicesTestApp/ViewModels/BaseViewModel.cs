using CommunityToolkit.Mvvm.ComponentModel;
using Controls.UserDialogs.Maui;
using Microsoft.Extensions.Logging;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;

namespace MedicoreMedicalServicesTestApp.ViewModels;

public abstract class BaseViewModel : ObservableObject,
    IInitializeAsyncAware,
    IInitializeAware,
    IDestructible
{
    public INavigationService NavigationService { get; }
    public IUserDialogs UserDialogs { get; }
    public ILogger Logger { get; }

    protected BaseViewModel(IServiceProvider serviceProvider)
    {
        NavigationService = serviceProvider.GetRequiredService<INavigationService>();
        UserDialogs = serviceProvider.GetRequiredService<IUserDialogs>();
        Logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(GetType()!);
    }

    public virtual Task InitializeAsync(INavigationParameters parameters)
    {
        return Task.CompletedTask;
    }

    public virtual void Initialize(INavigationParameters parameters)
    {

    }
    
    public virtual void Destroy()
    {
        
    }
}