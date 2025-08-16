namespace ShowMoBudAPI.DTO.DTOModels
{
    public class SurveyQuestionDto
    {
        public int SurveyQuestionId { get; set; }
        public string SurveyName { get; set; } = "";
        public string QuestionText { get; set; } = "";
        public string Option1 { get; set; } = "";
        public string Option2 { get; set; } = "";
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
    }
}
