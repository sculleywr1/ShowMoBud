namespace ShowMoBudAPI.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserProfile> Users { get; set; } = new List<UserProfile>();
}
