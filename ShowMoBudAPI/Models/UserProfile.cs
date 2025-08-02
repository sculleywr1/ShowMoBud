using System;
using System.Collections.Generic;

namespace ShowMoBudAPI.Models;

public partial class UserProfile
{
    public Guid UserId { get; set; }

    public string? Bio { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<SocialLink> SocialLinks { get; set; } = new List<SocialLink>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserPoint> UserPoints { get; set; } = new List<UserPoint>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
