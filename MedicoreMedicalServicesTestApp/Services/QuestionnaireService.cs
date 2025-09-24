using System.Text.Json;
using MedicoreMedicalServicesTestApp.Data;
using MedicoreMedicalServicesTestApp.Models;
using MedicoreMedicalServicesTestApp.Models.Enums;
using MedicoreMedicalServicesTestApp.Models.Observables;

namespace MedicoreMedicalServicesTestApp.Services;

public interface IQuestionnaireService
{
    Task<List<BaseQuestionObservable>> LoadQuestionsAsync();
    Task SaveResponsesAsync(List<BaseQuestionObservable> questions);
    Task<List<QuestionnaireResponses>> GetHistoryResponsesAsync();
}

public class QuestionnaireService : IQuestionnaireService
{
    private readonly IDbService _dbService;

    public QuestionnaireService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<List<BaseQuestionObservable>> LoadQuestionsAsync()
    {
        //TODO: Load from API or local DB
        await using var stream = await FileSystem.OpenAppPackageFileAsync("questionnaire.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
#if DEBUG
            WriteIndented = true,
#endif
        };
        var questions = JsonSerializer.Deserialize<List<Question>>(contents, jsonOptions) ?? [];

        return questions.Select<Question, BaseQuestionObservable?>(
                q =>
                {
                    switch (q.Type)
                    {
                        case QuestionType.Text:
                            return new TextQuestionObservable(q);
                        case QuestionType.Date:
                            return new DateQuestionObservable(q);
                        case QuestionType.Checkbox:
                            if (q.Args is not JsonElement args)
                                throw new ArgumentNullException("Args should not be null or empty for checkbox type");

                            var options = args.EnumerateArray().Select(x => x.Deserialize<CheckBoxArg>(jsonOptions)).Where(x => x != null).ToList()!;
                            return new CheckBoxQuestionObservable(q, options!);
                        default:
                            return null;
                    }
                }
            )
            .Where(x => x != null)
            .ToList()!;
    }

    public async Task SaveResponsesAsync(List<BaseQuestionObservable> questions)
    {
        await _dbService.RunInTransactionAsync(
            async () =>
            {
                var questionnaireResponses = new QuestionnaireResponsesDto
                {
                    CreatedAt = DateTime.UtcNow,
                };

                await _dbService.Insert(questionnaireResponses);

                var responses = questions.Select(
                    q =>
                    {
                        object? answer = q switch
                        {
                            TextQuestionObservable textQuestion => textQuestion.Answer,
                            DateQuestionObservable dateQuestion => dateQuestion.Answer,
                            CheckBoxQuestionObservable checkBoxQuestion => checkBoxQuestion.Options,
                            _ => null
                        };

                        return new ResponseDto
                        {
                            QuestionnaireResponseId = questionnaireResponses.Id!.Value,
                            QuestionId = q.Id.ToString(),
                            QuestionType = q.QuestionType,
                            QuestionText = q.Title,
                            AnswerJson = JsonSerializer.Serialize(answer)
                        };
                    }
                ).ToList();

                await _dbService.InsertAll(responses);
            }
        );
    }

    public async Task<List<QuestionnaireResponses>> GetHistoryResponsesAsync()
    {
        var questionnaireResponsesDtos = await _dbService.GetAll<QuestionnaireResponsesDto>();

        var results = new List<QuestionnaireResponses>();

        foreach (var questionnaireResponsesDto in questionnaireResponsesDtos.OrderBy(qr => qr.CreatedAt))
        {
            results.Add(
                new QuestionnaireResponses
                {
                    Id = questionnaireResponsesDto.Id!.Value,
                    CreatedAt = questionnaireResponsesDto.CreatedAt,
                    Responses = (await _dbService.GetBy<ResponseDto>(x => x.QuestionnaireResponseId == questionnaireResponsesDto.Id))
                        .Select(
                            r =>
                            {
                                object? answer;
                                
                                switch (r.QuestionType)
                                {
                                    case QuestionType.Text:
                                    {
                                        var str = JsonSerializer.Deserialize<string?>(r.AnswerJson);
                                        answer = string.IsNullOrWhiteSpace(str) ? null : str;
                                        break;
                                    }
                                    case QuestionType.Date:
                                        answer = JsonSerializer.Deserialize<DateTime?>(r.AnswerJson);
                                        break;
                                    case QuestionType.Checkbox:
                                        answer = JsonSerializer.Deserialize<CheckBoxOptionsList?>(r.AnswerJson);
                                        break;
                                    default:
                                        answer = null;
                                        break;
                                }

                                return new Response
                                {
                                    Id = r.Id,
                                    QuestionId = r.QuestionId,
                                    QuestionText = r.QuestionText,
                                    QuestionType = r.QuestionType,
                                    Answer = answer ?? "Not answered"
                                };
                            }
                        ).ToList()
                }
            );
        }

        return results;
    }
}