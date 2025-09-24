using CommunityToolkit.Mvvm.ComponentModel;
using MedicoreMedicalServicesTestApp.Models.Enums;

namespace MedicoreMedicalServicesTestApp.Models.Observables;

public abstract class BaseQuestionObservable : ObservableObject
{
    public Guid Id { get; }
    public string Title { get; }
    public string? Description { get; }
    public QuestionType QuestionType { get; }
    public bool IsRequired { get; set; }

    protected BaseQuestionObservable(Question question, QuestionType questionType)
    {
        Id = question.Id;
        Title = question.Title;
        Description = question.Description;
        IsRequired = question.IsRequired;
        QuestionType = questionType;
    }
}