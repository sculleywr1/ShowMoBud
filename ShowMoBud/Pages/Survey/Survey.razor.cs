using Microsoft.AspNetCore.Components;
using ShowMoBud.DTO;
using ShowMoBud.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ShowMoBud.Pages.Survey;

public partial class Survey : ComponentBase
{
    private List<string>? _surveyNames;
    private List<SurveyQuestionDto>? _questions;
    private readonly Dictionary<int, string> _answers = new();
    private NewsletterSignupDto _signup = new();
    private bool _showSignup;

    [Inject]
    protected ISurveyClient SurveyClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _surveyNames = await SurveyClient.GetSurveyNamesAsync();
    }

    protected async Task LoadSurvey(string name)
    {
        _questions = await SurveyClient.GetQuestionsAsync(name);
        _answers.Clear();
        foreach (var q in _questions)
        {
            _answers[q.SurveyQuestionId] = string.Empty;
        }
    }

    protected void ShowSignup()
    {
        if (_questions != null && _answers.All(a => !string.IsNullOrEmpty(a.Value)))
        {
            _showSignup = true;
        }
    }

    protected async Task SendSurvey()
    {
        var payload = _answers.Select(a => new SurveyResponseDto
        {
            SurveyQuestionId = a.Key,
            Answer = a.Value,
            Signup = _signup
        });
        await SurveyClient.SubmitAsync(payload);
        _showSignup = false;
        _questions = null;
        _answers.Clear();
        _signup = new();
    }
}
