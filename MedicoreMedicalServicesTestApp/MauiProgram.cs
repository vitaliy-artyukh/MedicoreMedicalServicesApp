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
                    var titleFont = "OpenSans-SemiBold.ttf";
                    var font = "OpenSans-Regular.ttf";
                    var buttonFont = "OpenSans-SemiBold.ttf";

                    AlertConfig.DefaultTitleFontFamily = titleFont;
                    AlertConfig.DefaultMessageFontFamily = font;
                    AlertConfig.DefaultPositiveButtonFontFamily = buttonFont;

                    ConfirmConfig.DefaultTitleFontFamily = titleFont;
                    ConfirmConfig.DefaultMessageFontFamily = font;
                    ConfirmConfig.DefaultNegativeButtonFontFamily = buttonFont;
                    ConfirmConfig.DefaultPositiveButtonFontFamily = buttonFont;

                    ActionSheetConfig.DefaultTitleFontFamily = titleFont;
                    ActionSheetConfig.DefaultMessageFontFamily = font;
                    ActionSheetConfig.DefaultOptionsButtonFontFamily = font;
                    ActionSheetConfig.DefaultDestructiveButtonFontFamily = font;
                    ActionSheetConfig.DefaultNegativeButtonFontFamily = buttonFont;

                    HudDialogConfig.DefaultMessageFontFamily = font;
                    HudDialogConfig.DefaultNegativeButtonFontFamily = buttonFont;

                    ToastConfig.DefaultMessageFontFamily = font;

                    SnackbarConfig.DefaultMessageFontFamily = font;
                    SnackbarConfig.DefaultNegativeButtonFontFamily = buttonFont;
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