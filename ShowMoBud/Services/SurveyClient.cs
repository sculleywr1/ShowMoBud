using ShowMoBud.DTO;
using ShowMoBud.Services.Interfaces;
using System.Net.Http.Json;

namespace ShowMoBud.Services;

public class SurveyClient : ISurveyClient
{
    private readonly HttpClient _http;
    private const string Base = "api/surveys";

    public SurveyClient(HttpClient http) => _http = http;

    public Task<List<string>> GetSurveyNamesAsync(CancellationToken ct = default) =>
        _http.GetFromJsonAsync<List<string>>(Base, ct)!;

    public Task<List<SurveyQuestionDto>> GetQuestionsAsync(string surveyName, CancellationToken ct = default) =>
        _http.GetFromJsonAsync<List<SurveyQuestionDto>>($"{Base}/{surveyName}", ct)!;

    public async Task SubmitAsync(IEnumerable<SurveyResponseDto> responses, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync(Base, responses, ct);
        resp.EnsureSuccessStatusCode();
    }
}
