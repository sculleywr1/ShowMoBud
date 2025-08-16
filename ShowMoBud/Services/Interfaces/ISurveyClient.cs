using ShowMoBud.DTO;

namespace ShowMoBud.Services.Interfaces;

public interface ISurveyClient
{
    Task<List<string>> GetSurveyNamesAsync(CancellationToken ct = default);
    Task<List<SurveyQuestionDto>> GetQuestionsAsync(string surveyName, CancellationToken ct = default);
    Task SubmitAsync(IEnumerable<SurveyResponseDto> responses, CancellationToken ct = default);
}
