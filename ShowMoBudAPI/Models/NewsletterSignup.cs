using System;
using System.Collections.Generic;

namespace ShowMoBudAPI.Models;

public partial class NewsletterSignup
{
    public int SignupId { get; set; }

    public string FirstName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public virtual ICollection<SurveyResponse> SurveyResponses { get; set; } = new List<SurveyResponse>();
}
