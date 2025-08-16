using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;

namespace ShowMoBudAPI.Services.Interfaces
{
    public interface ISurveyService
    {

        Task<List<string>> GetSurveyNamesAsync(CancellationToken ct = default);
        Task<List<SurveyQuestion>> GetQuestionsAsync(string surveyName, CancellationToken ct);
        Task<SurveyResponse> AddAsync(SurveyResponseDto dto, CancellationToken ct);
    }
}
