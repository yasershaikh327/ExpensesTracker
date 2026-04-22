using DataAccess.Repository.Interface;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpensesTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMemberRepository _memberRepository;

        public HomeController(IConfiguration configuration,IMemberRepository memberRepository)
        {
            _configuration = configuration;
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
            var data = _memberRepository.AddMember(registration);
            return Json(new { success = true, message = data });
        }

   
    }
}
