namespace MedicoreMedicalServicesTestApp.Models;

public class QuestionnaireResponses
{
    public int Id { get; set; }

    public List<Response> Responses { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}