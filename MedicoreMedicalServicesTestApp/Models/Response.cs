using MedicoreMedicalServicesTestApp.Models.Enums;

namespace MedicoreMedicalServicesTestApp.Models;

public class Response
{
    public int Id { get; set; }
    
    public string QuestionId { get; set; } = null!;
    
    public string QuestionText { get; set; } = null!;
    
    public QuestionType QuestionType { get; set; }
    
    public object? Answer { get; set; }
}