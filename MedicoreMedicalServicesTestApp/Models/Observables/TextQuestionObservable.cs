using CommunityToolkit.Mvvm.ComponentModel;
using MedicoreMedicalServicesTestApp.Models.Enums;

namespace MedicoreMedicalServicesTestApp.Models.Observables;

public partial class TextQuestionObservable : BaseQuestionObservable
{
    [ObservableProperty] private string _answer = string.Empty;
    
    public TextQuestionObservable(Question question)
        : base(question, QuestionType.Text)
    {

    }
}