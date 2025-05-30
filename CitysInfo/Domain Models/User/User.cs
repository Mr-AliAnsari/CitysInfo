using Microsoft.AspNetCore.Identity;

namespace CitysInfo.Domain_Models.User
{
    public class User : IdentityUser<int>
    {
        //public User(string userName)
        //{
        //    UserName = userName;
        //}

        public required string UserName { get; set; }

        public bool IsBanned { get; set; } // کاربر مسدود شده یا نه

        public bool IsDeleted { get; set; } // حذف نرم (Soft Delete)

        public string? FullName { get; set; } // نام کامل (nullable)

        public bool IsSystemic { get; set; } // آیا کاربر سیستمی است؟

        //public List<UserToken>? UserToken { get; set; }
    }
}
