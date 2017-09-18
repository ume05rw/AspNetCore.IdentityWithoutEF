using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthNoneEf.Models;

namespace AuthNoneEf.Controllers
{
    public class AuthController : AppController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
