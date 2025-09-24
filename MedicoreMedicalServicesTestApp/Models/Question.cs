using MedicoreMedicalServicesTestApp.Models.Enums;

namespace MedicoreMedicalServicesTestApp.Models;

public class Question
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public QuestionType Type { get; set; }
    
    public object? Args { get; set; }
    
    public bool IsRequired { get; set; }
}