using ExpensesTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpensesTracker.Controllers
{
    public class MemberController : Controller
    {
        private readonly IConfiguration _configuration;

        public MemberController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Transactions()
        {
            return View();
        }
        public IActionResult Budget()
        {
            return View();
        }
        public IActionResult Reports()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
