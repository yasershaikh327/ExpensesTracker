using BrevoEmailSender;
using DataAccess.DtoModels.Request;
using DataAccess.Helper.Interface;
using DataAccess.Mappers;
using DataAccess.Migrations;
using DataAccess.Models;
using DataAccess.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ExpensesTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IHomeRepository _homeRepository;
        private readonly IBrevoEmailService _emailService;
        private readonly BrevoOptions _options;
        private readonly IHelper _helper;
        public HomeController(IMemberRepository memberRepository, IHomeRepository homeRepository, IBrevoEmailService emailService, BrevoOptions options, IHelper helper)
        {
            _memberRepository = memberRepository;
            _homeRepository = homeRepository;
            _emailService = emailService;
            _options = options;
            _helper = helper;
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
                var subject = "Thanks for contacting Yaser Shaikh — I received your message\r\n";
                var subject_admin = "New Portfolio Contact from {{Name}}\r\n";
                var body = $@"
                <h2>Hello {contact.Name},</h2>

                <p>Thank you for reaching out through my portfolio website.</p>

                <p>I have received your message and will get back to you as soon as possible.</p>

                <h3>Your Message:</h3>
                <p style='background:#f4f4f4;padding:15px;border-radius:8px;'>
                {contact.Message}
                </p>

                <br/>

                <p>If your query is urgent, feel free to reply directly to this email.</p>

                <p>Best Regards,<br/>
                <b>Yaser Shaikh</b><br/>
                Software Engineer</p>
                ";
                var adminBody = $@"
                <h2>New Contact Form Submission</h2>

                <p><strong>Name:</strong> {contact.Name}</p>
                <p><strong>Email:</strong> {contact.Email}</p>

                <p><strong>Message:</strong></p>
                <p style='background:#f4f4f4;padding:15px;border-radius:8px;'>
                {contact.Message}
                </p>

                <hr/>
                <p>This message was sent from your portfolio website contact form.</p>
                ";

                _emailService.SendEmailAsync(contact.Email, contact.Name, subject, body);
                _emailService.SendEmailAsync("syaser327@gmail.com", "Yaser", subject_admin, adminBody);
                var senderEmail = Environment.GetEnvironmentVariable("DEFAULT_SENDER_EMAIL");
                var senderName = Environment.GetEnvironmentVariable("DEFAULT_SENDER_NAME");
                var log_Email = new log_email
                {
                    SenderEmail = senderEmail,
                    senderName = senderName,
                    recipientEmail = contact.Email,
                    recipientName = contact.Name,
                    Subject = subject,
                    htmlContent = contact.Message

                };
                _emailService.AddEmailLogs(log_Email);
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

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            if (ModelState.IsValid)
            {
                var Verify = _memberRepository.ResetPassword(resetPassword);
                if (Verify == 1)
                {
                    return Json(new { success = true, message = "Password Updated Successfully" });
                }
                else if (Verify == 0)
                {
                    return Json(new { success = false, message = "Invalid Old Password" });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid email or code" });
                }
            }
            return Json(new { success = false, message = "Please fill out all required fields correctly." });
        }

        [HttpPost]
        public JsonResult ForgetPassword([FromBody] ForgetPasswordDTO forgetPassword)
        {
            if(ModelState.IsValid)
            {
                var data = _memberRepository.CheckIfEmailExists(forgetPassword);
                var ReceiptName = _memberRepository.GetUserDetailsByEmail(forgetPassword.Email);
                if (data)
                {
                    var subject = "Password Reset Request\r\n";
                    var body = $@"
                    <h2>Hello {ReceiptName.Name ?? "User"},</h2>

                    <p>
                        Please
                        <a href='{Environment.GetEnvironmentVariable("CURRENT_DOMAIN")}/Home/ResetPassword?Code={Uri.EscapeDataString(_helper.HashPassword(forgetPassword.Email))}&Email={Uri.EscapeDataString(forgetPassword.Email)}'>
                            Click Here
                        </a>
                        to Reset Your Password.
                    </p>

                    <br/>

                    <p>Best Regards,<br/>
                    <b>Yaser Shaikh</b><br/>
                    Software Engineer</p>
                    ";
                    

                    _emailService.SendEmailAsync(forgetPassword.Email, ReceiptName.Name ?? "User", subject, body);
                    var senderEmail = Environment.GetEnvironmentVariable("DEFAULT_SENDER_MAIL");
                    var senderName = Environment.GetEnvironmentVariable("DEFAULT_SENDER_NAME");
                    var log_Email = new log_email
                    {
                        SenderEmail = senderEmail ?? "default@example.com",
                        senderName = senderName ?? "Default Sender",
                        recipientEmail = forgetPassword.Email,
                        recipientName = ReceiptName.Name ?? "User",
                        Subject = subject,
                        htmlContent = body

                    };
                    _emailService.AddEmailLogs(log_Email);
                    return Json(new { success = false, message = "Password Reset Link is sent to your Email Address: "+ forgetPassword.Email });
                }
                return Json(new { success = false, message = data });
            }
            return Json(new { success = false, message = "Please provide a valid email address." });
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
                    var token_data = _memberRepository.GetUserDetailsByToken(token);
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
