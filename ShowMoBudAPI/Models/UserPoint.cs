using System;
using System.Collections.Generic;

namespace ShowMoBudAPI.Models;

public partial class UserPoint
{
    public int PointId { get; set; }

    public Guid UserId { get; set; }

    public int TotalPoints { get; set; }

    public DateTime LastUpdated { get; set; }

    public virtual UserProfile User { get; set; } = null!;
}
