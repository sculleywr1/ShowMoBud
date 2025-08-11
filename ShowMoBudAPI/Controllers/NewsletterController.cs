// Controllers/NewsletterController.cs
using Microsoft.AspNetCore.Mvc;
using ShowMoBudAPI.Models;
using ShowMoBudAPI.Services;
using ShowMoBudAPI.Services.Interfaces;

[ApiController]
[Route("api/newsletter")]
public class NewsletterController : ControllerBase
{
    private readonly INewsletterService _service;
    public NewsletterController(INewsletterService service) => _service = service;

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] NewsletterSignup dto, CancellationToken ct)
    {
        try
        {
            var saved = await _service.AddAsync(dto, ct);
            return Ok(saved);
        }
        catch (InvalidOperationException ioe) when (ioe.InnerException != null)
        {
            return Conflict("Email already subscribed.");
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
    }

    [HttpGet]
    public Task<List<NewsletterSignup>> GetAll(CancellationToken ct) => _service.GetAllAsync(ct);
}
