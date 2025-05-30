using Microsoft.AspNetCore.Identity;

namespace CitysInfo.Domain_Models.User
{
    public class Role : IdentityRole<int>
    {
        public Role(string name)
        {
            Name = name;
        }
    }
}
