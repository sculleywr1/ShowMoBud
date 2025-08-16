using Microsoft.AspNetCore.Mvc;
using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Services.Interfaces;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace ShowMoBudAPI.Controllers;

[ApiController]
[Route("api/surveys")]
public class SurveyController : ControllerBase
{
    private readonly ISurveyService _service;
    public SurveyController(ISurveyService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<string>>> GetSurveyNames(CancellationToken ct) =>
        await _service.GetSurveyNamesAsync(ct);

    [HttpGet("{surveyName}")]
    public async Task<ActionResult<List<SurveyQuestionDto>>> GetQuestions(
        string surveyName, CancellationToken ct)
    {
        var q = await _service.GetQuestionsAsync(surveyName, ct);
        return q.Select(x => new SurveyQuestionDto
        {
            SurveyQuestionId = x.SurveyQuestionId,
            SurveyName = x.SurveyName,
            QuestionText = x.QuestionText,
            Option1 = x.Option1,
            Option2 = x.Option2,
            Option3 = x.Option3,
            Option4 = x.Option4
        }).ToList();
    }

    [HttpPost]
    public async Task<IActionResult> Submit(
        [FromBody] List<SurveyResponseDto> dtos, CancellationToken ct)
    {
        var validDtos = dtos
            .Where(dto => dto.SurveyQuestionId > 0 && !string.IsNullOrEmpty(dto.Answer))
            .ToList();

        if (!validDtos.Any())
        {
            return BadRequest(new { message = "No valid survey responses provided." });
        }

        try
        {
            await _service.AddResponsesAsync(validDtos, ct);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error processing survey responses", error = ex.Message });
        }
    }
}
