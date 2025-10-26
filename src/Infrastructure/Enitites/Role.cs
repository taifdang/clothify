using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Enitites;

public class Role : IdentityRole<Guid>
{
    public virtual ICollection<UserRole> UserRole { get; set; }
}
