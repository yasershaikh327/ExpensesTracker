using DataAccess.DtoModels;
using DataAccess.Models;
using DataAccess.Repository.Interface;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpensesTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemberRepository _memberRepository;

        public HomeController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
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
                var data = _memberRepository.Login(login);
                return Json(new { success = true, message = "Login Successful", token = data });
            }
            return Json(new { success = false, message = "Please fill out all required fields correctly." });
        }
    }
}
