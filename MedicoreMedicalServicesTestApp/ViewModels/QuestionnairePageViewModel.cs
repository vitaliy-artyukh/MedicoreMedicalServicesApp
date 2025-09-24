using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedicoreMedicalServicesTestApp.Models.Observables;
using MedicoreMedicalServicesTestApp.Services;
using MedicoreMedicalServicesTestApp.Views;
using Microsoft.Extensions.Logging;
using MPowerKit.Navigation.Interfaces;

namespace MedicoreMedicalServicesTestApp.ViewModels;

public partial class QuestionnairePageViewModel : BaseViewModel
{
    private readonly IQuestionnaireService _questionnaireService;

    [ObservableProperty] private List<BaseQuestionObservable>? _questions;

    public QuestionnairePageViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _questionnaireService = serviceProvider.GetRequiredService<IQuestionnaireService>();
    }

    public override async Task InitializeAsync(INavigationParameters parameters)
    {
        Questions = await _questionnaireService.LoadQuestionsAsync();

        foreach (var question in Questions)
        {
            question.PropertyChanged += OnQuestionPropertyChanged;

            if (question is not CheckBoxQuestionObservable checkBoxQuestion)
                continue;

            foreach (var option in checkBoxQuestion.Options)
                option.PropertyChanged += OnQuestionPropertyChanged;
        }
    }

    public override void Destroy()
    {
        if (Questions == null)
            return;

        foreach (var question in Questions)
        {
            question.PropertyChanged -= OnQuestionPropertyChanged;

            if (question is not CheckBoxQuestionObservable checkBoxQuestion)
                continue;

            foreach (var option in checkBoxQuestion.Options)
                option.PropertyChanged -= OnQuestionPropertyChanged;
        }
    }

    private void OnQuestionPropertyChanged(object? sender, PropertyChangedEventArgs e) =>
        SaveAnswerCommand.NotifyCanExecuteChanged();

    [RelayCommand]
    private async Task OnOpenHistoryAsync()
    {
        await NavigationService.NavigateAsync(nameof(HistoryPage));
    }

    [RelayCommand]
    private async Task OnSaveAnswerAsync()
    {
        try
        {
            if (!CanSave())
                return;
            
            using (UserDialogs.Loading())
            {
                await Task.Delay(2000);
                await _questionnaireService.SaveResponsesAsync(Questions!);

                foreach (var question in Questions!)
                {
                    switch (question)
                    {
                        case CheckBoxQuestionObservable checkBoxQuestionObservable:
                            foreach (var checkBoxOptionObservable in checkBoxQuestionObservable.Options)
                                checkBoxOptionObservable.IsChecked = false;
                            break;
                        case DateQuestionObservable dateQuestionObservable:
                            dateQuestionObservable.Answer = null;
                            break;
                        case TextQuestionObservable textQuestionObservable:
                            textQuestionObservable.Answer = string.Empty;
                            break;
                    }
                }
            }

            await UserDialogs.AlertAsync("Responses saved successfully", "Success", "OK");
        }
        catch (Exception e)
        {
            Logger.LogDebug(e, "Failed to save responses");
            await UserDialogs.AlertAsync("Failed to save responses", "Error", "OK");
        }
    }

    private bool CanSave()
    {
        if (Questions == null || Questions.Count == 0)
            return false;

        var canSave = false;
        var hasAnyRequired = false;

        foreach (var question in Questions)
        {
            if (!question.IsRequired)
                continue;

            hasAnyRequired = true;

            canSave = question switch
            {
                TextQuestionObservable textQuestion => !string.IsNullOrEmpty(textQuestion.Answer),
                DateQuestionObservable dateQuestion => dateQuestion.Answer != null,
                CheckBoxQuestionObservable checkBoxQuestion => checkBoxQuestion.Options.Any(o => o.IsChecked),
                _ => canSave
            };

            if (!canSave)
                break;
        }

        return !hasAnyRequired || hasAnyRequired && canSave;
    }
}