using System;
using System.Collections.Generic;

namespace ShowMoBudAPI.Models;

public partial class SurveyResponse
{
    public int SurveyResponseId { get; set; }

    public int SurveyQuestionId { get; set; }

    public string Answer { get; set; } = null!;

    public int SignupId { get; set; }

    public virtual NewsletterSignup Signup { get; set; } = null!;

    public virtual SurveyQuestion SurveyQuestion { get; set; } = null!;
}
