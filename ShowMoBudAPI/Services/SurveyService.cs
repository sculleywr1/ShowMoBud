using Microsoft.EntityFrameworkCore;
using ShowMoBudAPI.Contexts;
using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;
using ShowMoBudAPI.Services.Interfaces;

namespace ShowMoBudAPI.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ShowMoBudContext _db;
        public SurveyService(ShowMoBudContext db) => _db = db;

        public Task<List<SurveyQuestion>> GetQuestionAsync(string surveyName, CancellationToken ct = default) =>
            _db.SurveyQuestions.Where(q => q.SurveyName == surveyName).ToListAsync(ct);

        public async Task<SurveyResponse> AddAsync(SurveyResponseDto dto, CancellationToken ct = default)
        {
            var signup = dto.Signup;
            _db.NewsletterSignups.Attach(signup);
            if (signup.SignupId == 0) _db.NewsletterSignups.Add(signup);

            var entity = new SurveyResponse
            {
                SurveyQuestionId = dto.SurveyQuestionId,
                Answer = dto.Answer,
                Signup = signup
            };
            _db.SurveyResponses.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public Task<List<string>> GetSurveyNamesAsync(CancellationToken ct = default) =>
            _db.SurveyQuestions.Select(q => q.SurveyName).Distinct().ToListAsync(ct);
    }
}
