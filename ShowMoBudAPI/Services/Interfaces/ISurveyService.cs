using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;

namespace ShowMoBudAPI.Services.Interfaces
{
    public interface ISurveyService
    {

        Task<List<SurveyQuestion>> GetQuestionAsync(string surveyName, CancellationToken ct);
        Task<SurveyResponse> AddAsync(SurveyResponseDto dto, CancellationToken ct);

    }
}
