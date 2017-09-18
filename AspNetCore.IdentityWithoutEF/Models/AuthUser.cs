using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthNoneEf.Models
{
    public class AuthUser : AppModel
    {
        public AuthUser()
        {
            this.Id = Guid.NewGuid();
        }

        public AuthUser(string loginName) : this()
        {
            this.LoginName = loginName;
        }

        public Guid Id { get; set; }

        public string LoginName { get; set; }

        public string NormalizedLoginName { get; set; }

        public string ScreenName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string UserId => this.Id.ToString();
    }
}
