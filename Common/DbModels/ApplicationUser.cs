using Microsoft.AspNetCore.Identity;

namespace Common.DbModels
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}
