using Microsoft.EntityFrameworkCore;
using ShowMoBudAPI.Contexts;
using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;
using ShowMoBudAPI.Services.Interfaces;
using System.Linq;
using System.Threading;

namespace ShowMoBudAPI.Services;

public class SurveyService : ISurveyService
{
    private readonly ShowMoBudContext _db;
    public SurveyService(ShowMoBudContext db) => _db = db;

    public Task<List<string>> GetSurveyNamesAsync(CancellationToken ct = default) =>
        _db.SurveyQuestions.Select(q => q.SurveyName).Distinct().ToListAsync(ct);

    public Task<List<SurveyQuestion>> GetQuestionsAsync(string surveyName, CancellationToken ct = default) =>
        _db.SurveyQuestions.Where(q => q.SurveyName == surveyName).ToListAsync(ct);

    public async Task AddResponsesAsync(IEnumerable<SurveyResponseDto> dtos, CancellationToken ct = default)
    {
        var dtoSignup = dtos.First().Signup;
        var signup = new NewsletterSignup
        {
            SignupId = dtoSignup.SignupId,
            FirstName = dtoSignup.FirstName,
            Email = dtoSignup.Email,
            PhoneNumber = dtoSignup.PhoneNumber
        };

        _db.NewsletterSignups.Attach(signup);
        if (signup.SignupId == 0)
            _db.NewsletterSignups.Add(signup);

        foreach (var dto in dtos)
        {
            _db.SurveyResponses.Add(new SurveyResponse
            {
                SurveyQuestionId = dto.SurveyQuestionId,
                Answer = dto.Answer,
                Signup = signup
            });
        }

        await _db.SaveChangesAsync(ct);
    }
}
