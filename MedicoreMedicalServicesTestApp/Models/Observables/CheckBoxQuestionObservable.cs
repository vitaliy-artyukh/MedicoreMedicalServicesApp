using CommunityToolkit.Mvvm.ComponentModel;
using MedicoreMedicalServicesTestApp.Models.Enums;

namespace MedicoreMedicalServicesTestApp.Models.Observables;

public class CheckBoxQuestionObservable : BaseQuestionObservable
{
    public CheckBoxOptionsList Options { get; }

    public CheckBoxQuestionObservable(Question question, List<CheckBoxArg> options)
        : base(question, QuestionType.Checkbox)
    {
        Options = new CheckBoxOptionsList(options.Select(o => new CheckBoxOptionObservable(o.Id, o.Text)));
    }
}

public partial class CheckBoxOptionObservable : ObservableObject
{
    [ObservableProperty] private bool _isChecked;

    public Guid Id { get; }
    public string Text { get; }

    public CheckBoxOptionObservable(Guid id, string text)
    {
        Id = id;
        Text = text;
    }
}

public class CheckBoxOptionsList : List<CheckBoxOptionObservable>
{
    public CheckBoxOptionsList()
    {
    }
    public CheckBoxOptionsList(IEnumerable<CheckBoxOptionObservable> collection) : base(collection)
    {
    }
    public CheckBoxOptionsList(int capacity) : base(capacity)
    {
    }
}