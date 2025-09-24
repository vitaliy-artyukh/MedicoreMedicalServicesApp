using CommunityToolkit.Mvvm.ComponentModel;
using MedicoreMedicalServicesTestApp.Models.Enums;

namespace MedicoreMedicalServicesTestApp.Models.Observables;

public partial class DateQuestionObservable : BaseQuestionObservable
{
    [ObservableProperty] private DateTime? _answer;

    public DateQuestionObservable(Question question)
        : base(question, QuestionType.Date)
    {
    }
}