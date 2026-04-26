using DataAccess.DtoModels;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpensesTracker.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMemberRepository _memberRepository;

        public MemberController(IConfiguration configuration, IMemberRepository memberRepository)
        {
            _configuration = configuration;
            _memberRepository = memberRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Expense([FromBody]ExpenseDTO expense)
        {
            if (ModelState.IsValid)
            {
                var token_data = _memberRepository.GetUserDetails(Request.Cookies["jwtToken"].ToString());
                expense.UserId = Convert.ToInt32(token_data.UserId);
                var data = _memberRepository.AddExpense(expense);
                return Json(new { success = true, message = data });
            }
            return Json(new { success = false, message = "Please fill out all required fields correctly. Ensure Data is Entered in proper Format" });
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
