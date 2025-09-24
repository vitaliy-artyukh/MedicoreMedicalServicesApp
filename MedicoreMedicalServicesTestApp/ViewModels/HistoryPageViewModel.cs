using CommunityToolkit.Mvvm.ComponentModel;
using MedicoreMedicalServicesTestApp.Models;
using MedicoreMedicalServicesTestApp.Services;
using Microsoft.Extensions.Logging;
using MPowerKit.Navigation.Interfaces;

namespace MedicoreMedicalServicesTestApp.ViewModels;

public partial class HistoryPageViewModel : BaseViewModel
{
    private readonly IQuestionnaireService _questionnaireService;

    [ObservableProperty] 
    private List<QuestionnaireResponses>? _historyResponses;

    public HistoryPageViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _questionnaireService = serviceProvider.GetRequiredService<IQuestionnaireService>();
    }

    public override async Task InitializeAsync(INavigationParameters parameters)
    {
        try
        {
            using var _ = UserDialogs.Loading();
            HistoryResponses = await _questionnaireService.GetHistoryResponsesAsync();
        }
        catch (Exception e)
        {
            Logger.LogDebug(e, "Error loading history responses");
            await UserDialogs.AlertAsync(e.Message, "Error", "OK");
        }
    }
}