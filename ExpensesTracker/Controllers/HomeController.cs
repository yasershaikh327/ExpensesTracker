using DataAccess.DtoModels;
using DataAccess.Models;
using DataAccess.Repository.Interface;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ExpensesTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IHomeRepository _homeRepository;

        public HomeController(IMemberRepository memberRepository, IHomeRepository homeRepository)
        {
            _memberRepository = memberRepository;
            _homeRepository = homeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Contact([FromBody]ContactDTO contact)
        {
            if (ModelState.IsValid)
            {
                var data = _homeRepository.Contact(contact);
                return Json(new { success = true, message = data });
            }
            return Json(new { success = false, message = "Please fill out all required fields correctly. Ensure Data is Entered in proper Format" });
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register([FromBody]RegistrationDTO registration)
        {
            if (ModelState.IsValid)
            {
                var data = _memberRepository.AddMember(registration);
                return Json(new { success = true, message = data });
            }
            return Json(new { success = false, message = "Please fill out all required fields correctly." });
        }

        [HttpPost]
        public JsonResult Login([FromBody]LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                var token = _memberRepository.Login(login);
                if (token != null)
                {
                    var token_data = _memberRepository.GetUserDetails(token);
                    Response.Cookies.Append("MemberName", token_data.Name);
                    return Json(new { success = true, message = "Login Successful", token = token });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false, message = "Please fill out all required fields correctly." });
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Login","Home");
        }
    }
}
