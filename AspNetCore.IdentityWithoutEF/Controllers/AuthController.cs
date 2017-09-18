using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AuthNoneEf.Models;
using AuthNoneEf.Models.Api.ErrorResponse;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace AuthNoneEf.Controllers
{
    public class AuthController : AppController
    {
        private Models.Auth _auth;

        public AuthController(UserManager<AuthUser> userManager,
                              SignInManager<AuthUser> signInManager)
        {
            this._auth = Models.Auth.GetInstance(userManager, signInManager);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Api()
        {
            try
            {
                this.Out("Headers: \r\n" + string.Join("\r\n", this.Request.Headers.Select(h => h.Key + ": " + h.Value)));

                var queryJson = this.Request.Query
                                            .Select(q => q.Key)
                                            .FirstOrDefault(k => k.IndexOf('{') >= 0
                                                                 && k.IndexOf('}') >= 0);

                var bodyString = Xb.Str.GetString(this.Request.Body);
                var passingString = !string.IsNullOrEmpty(bodyString)
                    ? bodyString
                    : Xb.Net.Http.DecodeUri(queryJson);
                
                Dictionary<string, string> dictionary;
                try
                {
                    dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(passingString);
                }
                catch (Exception)
                {
                    return Json(new ParseErrorResponse(bodyString));
                }

                switch (this.Request.Method.ToUpper())
                {
                    case "GET":
                        return Json(await this._auth.SignInAsync(dictionary));

                    case "POST":
                    case "PUT":
                    case "PATCH":

                        return Json(await this._auth.CreateAsync(dictionary));

                    case "DELETE":
                        return Json(await this._auth.SignOutAsync());

                    case "HEAD":
                    case "CONNECT":
                    case "OPTIONS":
                    case "TRACE":
                    default:
                        throw new NotImplementedException($"Undefineded method: {this.Request.Method}");
                }

                return Json(false);
            }
            catch (Exception ex)
            {
                this.Out(ex);
                return Json(new UnexpectedErrorResponse(ex));
            }
        }
    }
}
