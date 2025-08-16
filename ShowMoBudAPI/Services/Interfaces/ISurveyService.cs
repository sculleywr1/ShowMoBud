using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;
using System.Collections.Generic;
using System.Threading;

namespace ShowMoBudAPI.Services.Interfaces;

public interface ISurveyService
{
    Task<List<string>> GetSurveyNamesAsync(CancellationToken ct = default);
    Task<List<SurveyQuestion>> GetQuestionsAsync(string surveyName, CancellationToken ct = default);
    Task AddResponsesAsync(IEnumerable<SurveyResponseDto> dtos, CancellationToken ct = default);
}
