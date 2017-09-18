using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AuthNoneEf.Models
{
    public class AuthUserStore : AppModel, IUserStore<AuthUser>, IUserPasswordStore<AuthUser>
    {
        private static Xb.Db.Sqlite Db;
        private static Xb.Db.Model Users;
        private static Xb.Db.Sqlite GetDb()
        {
            if (AuthUserStore.Db != null)
                return AuthUserStore.Db;

            AuthUserStore.Db = new Xb.Db.Sqlite("");
            return AuthUserStore.Db;
        }
        private static Xb.Db.Model GetUserModel()
        {
            if (AuthUserStore.Users != null)
                return AuthUserStore.Users;

            var db = AuthUserStore.GetDb();
            AuthUserStore.Users = db.Models["users"];
            return AuthUserStore.Users;
        }



        private Xb.Db.Sqlite _db;
        private Xb.Db.Model _users;

        public AuthUserStore()
        {

        }

        public Task<IdentityResult> CreateAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<AuthUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<AuthUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(AuthUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(AuthUser user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(AuthUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
