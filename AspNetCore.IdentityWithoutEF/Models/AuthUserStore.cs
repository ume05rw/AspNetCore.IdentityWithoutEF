using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Xb.Db;

namespace AuthNoneEf.Models
{
    public class AuthUserStore : AppDbModel, IUserStore<AuthUser>, IUserPasswordStore<AuthUser>
    {
        #region "AppDbModel Implements"
        public override string TableName => "Users";

        protected override Dictionary<string, string> DbColumnDefinitions => new Dictionary<string, string>()
        {
            { "Id" , "TEXT NOT NULL" },
            { "LoginName" , "TEXT NOT NULL" },
            { "NormalizedLoginName" , "TEXT NOT NULL" },
            { "ScreenName" , "TEXT" },
            { "Email" , "TEXT" },
            { "PasswordHash" , "TEXT" },
        };

        protected override List<string> DbAdditionalDefinitions => new List<string>()
        {
            "PRIMARY KEY (Id)"
        };

        protected override void FormatDbInitData(Sqlite db)
        {
            //初期値はナシ
        }
        #endregion "AppDbModel Implements"

        private static IdentityErrorDescriber IdentityErrorDescriber
            = new IdentityErrorDescriber();


        private Xb.Db.ResultRow GetRow(AuthUser user = null)
        {
            var row = this.DbModel.NewRow();
            if (user == null)
                return row;

            row["Id"] = user.UserId;
            row["LoginName"] = user.LoginName;
            row["NormalizedLoginName"] = user.NormalizedLoginName;
            row["ScreenName"] = user.ScreenName;
            row["Email"] = user.Email;
            row["PasswordHash"] = user.PasswordHash;

            return row;
        }

        private AuthUser GetUser(Xb.Db.ResultRow row)
        {
            var user = new AuthUser();
            user.Id = Guid.Parse(row.Get<string>("Id"));
            user.LoginName = row.Get<string>("LoginName");
            user.NormalizedLoginName = row.Get<string>("NormalizedLoginName");
            user.ScreenName = row.Get<string>("ScreenName");
            user.Email = row.Get<string>("Email");
            user.PasswordHash = row.Get<string>("PasswordHash");

            return user;
        }

        /// <summary>
        /// ユーザー追加
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateAsync(AuthUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var row = this.GetRow(user);
            var result = this.DbModel.Write(row);

            if (result.Length > 0)
            {
                return Task.FromResult(
                    IdentityResult.Failed(IdentityErrorDescriber.ConcurrencyFailure())
                );
            }

            return Task.FromResult(IdentityResult.Success);
        }

        /// <summary>
        /// 更新する。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> UpdateAsync(AuthUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var row = this.GetRow(user);
            var result = this.DbModel.Write(row);

            if (result.Length > 0)
            {
                return Task.FromResult(
                    IdentityResult.Failed(IdentityErrorDescriber.ConcurrencyFailure())
                );
            }

            return Task.FromResult(IdentityResult.Success);
        }

        /// <summary>
        /// ユーザー削除
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> DeleteAsync(AuthUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var row = this.GetRow(user);
            var result = this.DbModel.Delete(row);

            if (result.Length > 0)
            {
                return Task.FromResult(
                    IdentityResult.Failed(IdentityErrorDescriber.ConcurrencyFailure())
                );
            }

            return Task.FromResult(IdentityResult.Success);
        }

        /// <summary>
        /// ユーザーをIDで検索する。
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<AuthUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var row = this.DbModel.Find(userId);
            if (row == null)
                return null;

            var user = this.GetUser(row);

            return Task.FromResult(user);
        }

        /// <summary>
        /// ユーザーを名前で検索する。
        /// </summary>
        /// <param name="normalizedUserName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<AuthUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (string.IsNullOrEmpty(normalizedUserName))
                throw new ArgumentNullException(nameof(normalizedUserName));

            var where = $"NormalizedLoginName = { this.Db.Quote(normalizedUserName) }";
            var table = this.DbModel.FindAll(where);
            if (table.RowCount <= 0)
                return null;

            var user = this.GetUser(table.Rows[0]);

            return Task.FromResult(user);
        }

        /// <summary>
        /// ユーザーの平坦化名称を取得する。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetNormalizedUserNameAsync(AuthUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.NormalizedLoginName);
        }

        /// <summary>
        /// ユーザーの平坦化名称をセットする。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetNormalizedUserNameAsync(AuthUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.NormalizedLoginName = normalizedName;

            var row = this.GetRow(user);
            var result = this.DbModel.Write(row);

            if (result.Length > 0)
            {
                return Task.FromResult(
                    IdentityResult.Failed(IdentityErrorDescriber.ConcurrencyFailure())
                );
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// ユーザIDを取得する。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserIdAsync(AuthUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserId);
        }

        /// <summary>
        /// ユーザー名を取得する。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserNameAsync(AuthUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.LoginName);
        }

        /// <summary>
        /// ユーザー名をセットする。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetUserNameAsync(AuthUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.LoginName = userName;

            var row = this.GetRow(user);
            var result = this.DbModel.Write(row);

            if (result.Length > 0)
            {
                return Task.FromResult(
                    IdentityResult.Failed(IdentityErrorDescriber.ConcurrencyFailure())
                );
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// パスワードを保持しているか否か
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(AuthUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash != null);
        }

        /// <summary>
        /// パスワードハッシュを取得する。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(AuthUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// パスワードハッシュをセットする。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(AuthUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.PasswordHash = passwordHash;

            var row = this.GetRow(user);
            var result = this.DbModel.Write(row);

            if (result.Length > 0)
            {
                return Task.FromResult(
                    IdentityResult.Failed(IdentityErrorDescriber.ConcurrencyFailure())
                );
            }

            return Task.CompletedTask;
        }
    }
}
