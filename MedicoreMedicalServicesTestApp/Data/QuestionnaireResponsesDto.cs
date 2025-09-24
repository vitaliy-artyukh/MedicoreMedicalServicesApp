using SQLite;

namespace MedicoreMedicalServicesTestApp.Data;

[Table("QuestionnaireResponses")]
public class QuestionnaireResponsesDto
{
    [PrimaryKey, AutoIncrement]
    public int? Id { get; set; }

    [Ignore]
    public List<ResponseDto> Responses { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}