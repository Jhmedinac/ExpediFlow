using Microsoft.AspNetCore.Identity;

namespace ExpediFlow.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }

    }
}
