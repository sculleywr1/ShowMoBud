using System;
using System.Collections.Generic;

namespace ShowMoBudAPI.Models;

public partial class SurveyQuestion
{
    public int SurveyQuestionId { get; set; }

    public string SurveyName { get; set; } = null!;

    public string QuestionText { get; set; } = null!;

    public string Option1 { get; set; } = null!;

    public string Option2 { get; set; } = null!;

    public string? Option3 { get; set; }

    public string? Option4 { get; set; }

    public virtual ICollection<SurveyResponse> SurveyResponses { get; set; } = new List<SurveyResponse>();
}
