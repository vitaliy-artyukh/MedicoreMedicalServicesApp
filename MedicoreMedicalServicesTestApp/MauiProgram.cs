using Controls.UserDialogs.Maui;
using MedicoreMedicalServicesTestApp.Services;
using MedicoreMedicalServicesTestApp.ViewModels;
using MedicoreMedicalServicesTestApp.Views;
using Microsoft.Extensions.Logging;
using MPowerKit.Navigation;
using MPowerKit.Navigation.Utilities;
using MPowerKit.VirtualizeListView;

namespace MedicoreMedicalServicesTestApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMPowerKitListView()
            .UseMPowerKitNavigation(
                mPowerBuilder =>
                {
                    mPowerBuilder
                        .ConfigureServices(RegisterTypes)
                        .OnAppStart($"{nameof(NavigationPage)}/{nameof(QuestionnairePage)}");
                }
            )
            .UseUserDialogs(
                true,
                static () =>
                {
                    
                }
            )
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }
            );

#if DEBUG
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void RegisterTypes(IServiceCollection services)
    {
        services.AddSingleton<IDbService, DbService>();
        services.AddSingleton<IQuestionnaireService, QuestionnaireService>();

        //Pages
        services.RegisterForNavigation<QuestionnairePage, QuestionnairePageViewModel>();
        services.RegisterForNavigation<HistoryPage, HistoryPageViewModel>();
    }
}