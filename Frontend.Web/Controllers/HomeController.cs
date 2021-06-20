using Frontend.Web.Models;
using MediatR;
using Membership.API.CQRS.Commands.Request;
using Membership.API.CQRS.Queries.Request;
using Membership.API.CQRS.Queries.Response;
using Membership.API.Models;
using Membership.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frontend.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly IMediator _mediator;
        private readonly SignInManager<User> _signInManager;

        public HomeController(IMediator mediator, SignInManager<User> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(GetUserQueryRequest loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            GetUserQueryResponse user = await _mediator.Send(loginModel);

            if(user.IsSuccess == false)
            {
                ModelState.AddModelError("", user.Message);
                return View();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> MyReferenceDetails()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            GetByIdUserQueryRequest user = new GetByIdUserQueryRequest
            {
                Email = User.Identity.Name
            };
            GetByIdUserQueryResponse userdetails = await _mediator.Send(user);

            return View(userdetails);
        }

        [HttpGet]
        [Route("home/register/{signupRefCode?}")]
        public IActionResult Register(string signupRefCode)
        {
            ViewBag.Id = signupRefCode;
            return View();
        }

        [HttpPost]
        [Route("home/register/{signupRefCode?}")]
        public IActionResult Register(CreateUserCommandRequest registerModel, string signupRefCode)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:25244/user/signup/");

                var signup = client.PostAsJsonAsync("", registerModel);
                signup.Wait();
                var result = signup.Result;

                if (result.IsSuccessStatusCode)
                {
                    TempData["message"] = "Register successful";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Content.ReadAsStringAsync().Result);
                    return View();
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
