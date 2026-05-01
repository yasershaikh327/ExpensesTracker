using DataAccess.DtoModels.Request;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpensesTracker.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberRepository _memberRepository;

        public MemberController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDashboardData()
        {
            var token_data = _memberRepository.GetUserDetailsByToken(Request.Cookies["jwtToken"].ToString());
            var data = _memberRepository.GetDashboardData(Convert.ToInt32(token_data.UserId));
            return Json(new { success = true, data });
        }

        [HttpPost]
        public JsonResult Expense([FromBody]ExpenseDTO expense)
        {
            if (ModelState.IsValid)
            {
                var token_data = _memberRepository.GetUserDetailsByToken(Request.Cookies["jwtToken"].ToString());
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
