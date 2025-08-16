namespace ShowMoBud.DTO
{
    public class SurveyResponseDto
    {
        public int SurveyQuestionId { get; set; }
        public string Answer { get; set; } = "";
        public NewsletterSignupDto Signup { get; set; } = new();
    }
}
