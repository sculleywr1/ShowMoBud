namespace ShowMoBudAPI.Models;

public partial class SocialLink
{
    public int LinkId { get; set; }

    public Guid UserId { get; set; }

    public string? Platform { get; set; }

    public string? Url { get; set; }

    public virtual UserProfile User { get; set; } = null!;
}
