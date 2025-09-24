using MedicoreMedicalServicesTestApp.Models.Enums;
using SQLite;

namespace MedicoreMedicalServicesTestApp.Data;

[Table("Responses")]
public class ResponseDto
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int QuestionnaireResponseId { get; set; } // Foreign key to link to QuestionnaireResponses
    public string QuestionId { get; set; } = null!;
    public string QuestionText { get; set; } = null!;
    public QuestionType QuestionType { get; set; }
    public string AnswerJson { get; set; } = null!;
}