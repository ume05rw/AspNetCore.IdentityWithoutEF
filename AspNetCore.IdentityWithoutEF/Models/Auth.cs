using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace AuthNoneEf.Models
{
    public class Auth
    {
        private static Auth _instance;
        public static Auth GetInstance(UserManager<AuthUser> userManager,
                                       SignInManager<AuthUser> signInManager)
        {
            if (Auth._instance != null)
            {
                Auth._instance.SetDependencies(userManager, signInManager);
                return Auth._instance;
            }

            Auth._instance = new Auth();
            Auth._instance.SetDependencies(userManager, signInManager);

            return Auth._instance;
        }



        private UserManager<AuthUser> _userManager;
        private SignInManager<AuthUser> _signInManager;

        private Auth()
        {
        }

        private void SetDependencies(UserManager<AuthUser> userManager,
                                     SignInManager<AuthUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<Api.Response> CreateAsync(Dictionary<string, string> dictionary)
        {
            var response = new Api.Response();
            var errList = response.Errors;

            if (!dictionary.ContainsKey("LoginName"))
                errList.Add(new Api.Error()
                {
                    Item = "LoginName",
                    Code = Api.ErrorCode.ItemNotFound,
                    Message = "LoginName not found."
                });

            if (!dictionary.ContainsKey("Password"))
                errList.Add(new Api.Error()
                {
                    Item = "Password",
                    Code = Api.ErrorCode.ItemNotFound,
                    Message = "Password not found"
                });

            if (!dictionary.ContainsKey("PasswordRetype"))
                errList.Add(new Api.Error()
                {
                    Item = "PasswordRetype",
                    Code = Api.ErrorCode.ItemNotFound,
                    Message = "PasswordRetype not found"
                });

            //項目が一つでも不足しているとき、エラー応答。
            if (errList.Count > 0)
                return response;

            if (dictionary["Password"] != dictionary["PasswordRetype"])
            {
                errList.Add(new Api.Error()
                {
                    Item = "PasswordRetype",
                    Code = Api.ErrorCode.ValueNotSame,
                    Message = "Password and PasswordRetype value not same."
                });
                return response;
            }

            var password = dictionary["Password"];
            var user = new AuthUser()
            {
                LoginName = dictionary["LoginName"],
                NormalizedLoginName = this._userManager.NormalizeKey(dictionary["LoginName"]),
                ScreenName = dictionary["LoginName"],
            };
            var createResult = await this._userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                errList.AddRange(createResult.Errors.Select(e => new Api.Error()
                {
                    Item = "",
                    //Code = e.Code,
                    Message = e.Description
                }));

                return response;
            }

            response.Succeeded = true;
            return response;
        }
    }
}
