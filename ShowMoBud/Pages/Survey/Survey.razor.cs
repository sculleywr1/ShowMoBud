using Microsoft.AspNetCore.Components;
using ShowMoBud.DTO;

namespace ShowMoBud.Pages.Survey;

public partial class Survey : ComponentBase
{
    protected SurveyResponseDto _submission = new() { SurveyName = "LandingSurvey" };
    protected string? _answer;
    protected bool _showSignup;

    protected void ShowSignup()
    {
        if (!string.IsNullOrEmpty(_answer))
        {
            _submission.Answer = _answer;
            _showSignup = true;
        }
    }

    protected async Task SendSurvey()
    {
        await SurveyClient.SubmitAsync(_submission);
        _showSignup = false;
    }

    [Inject]
    protected ISurveyClient SurveyClient { get; set; } = default!;
}