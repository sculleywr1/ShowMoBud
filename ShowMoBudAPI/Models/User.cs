using System;
using System.Collections.Generic;

namespace ShowMoBudAPI.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public string? ProfileImageUrl { get; set; }

    public DateTime JoinDate { get; set; }

    public bool IdVerificationStatus { get; set; }

    public bool IsActive { get; set; }

    public virtual UserProfile? UserProfile { get; set; }
}
