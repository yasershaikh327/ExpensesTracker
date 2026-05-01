using AutoMapper;
using DataAccess.DtoModels.Request;
using DataAccess.DtoModels.Response;
using DataAccess.Helper.Interface;
using DataAccess.Mappers.Interface;
using DataAccess.Models;
using DataAccess.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DbPostgreContext _postgreContext;
        private readonly IRegistrationMapper _registrationMapper;
        private readonly ILoginMapper _loginMapper;
        private readonly IExpenseMapper _expenseMapper;
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration; 
        public MemberRepository(DbPostgreContext postgreContext, IRegistrationMapper registrationMapper, ILoginMapper loginMapper, IExpenseMapper expenseMapper, IHelper helper, IConfiguration configuration) 
        { 
            _postgreContext = postgreContext;
            _registrationMapper = registrationMapper;
            _loginMapper = loginMapper;
            _helper = helper;
            _configuration = configuration;
            _expenseMapper = expenseMapper; 
        }

        public string AddMember(RegistrationDTO registrationDto)
        {
            registrationDto.Password = _helper.HashPassword(registrationDto.Password!=null ? registrationDto.Password : string.Empty);
            var mapper =  _registrationMapper.Map(registrationDto);          
            _postgreContext.registrations.Add(mapper);
            _postgreContext.SaveChanges();
            return "Member added successfully. Redirecting to Login Page";
        }

        public string Login(LoginDTO loginDto)
        {
            var mapper =  _loginMapper.Map(loginDto); 
            var existingUser = _postgreContext.registrations.FirstOrDefault(r => r.Email == mapper.Email);
            if (existingUser != null && _helper.VerifyPassword(mapper.Password, existingUser.Password))
            {
                var LoginLogs = new LoginLogs()
                {
                    UserId = existingUser.Id,
                    LoginTime = DateTime.UtcNow
                };
                existingUser.LastLogin = DateTime.UtcNow;  
                _postgreContext.loginLogs.Add(LoginLogs);
                _postgreContext.SaveChanges();
                return GenerateToken(existingUser!.Email, existingUser!.Name, existingUser!.Id);
            }
            return null;
        }

        public string GenerateToken(string email, string name, int id)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Key"]));

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["DurationInMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenData GetUserDetailsByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            token = token.Replace("Bearer ", "").Trim();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(x =>
                x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            var email = jwtToken.Claims.FirstOrDefault(x =>
                x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            var name = jwtToken.Claims.FirstOrDefault(x =>
                x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            var role = jwtToken.Claims.FirstOrDefault(x =>
                x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            var tokenData = new TokenData
            {
                Email = email,
                Name = name,
                UserId = userId,
                Role = role
            };

            return tokenData;
        }

        public string AddExpense(ExpenseDTO expense)
        {
            var mapper = _expenseMapper.Map(expense);
            _postgreContext.Add(mapper);
            _postgreContext.SaveChanges();
            return "Expense added successfully.";
        }

        public bool CheckIfEmailExists(ForgetPasswordDTO forgetPassword)
        {
            var existingUser = _postgreContext.registrations.FirstOrDefault(r => r.Email == forgetPassword.Email);
            if (existingUser != null)
            {
                return true;
            }
            return false;
        }

        public RegistrationDTO GetUserDetailsByEmail(string Email)
        {
            var existingUser = _postgreContext.registrations.FirstOrDefault(r => r.Email == Email);
            if (existingUser != null)
            {
                 var registrationDto = new RegistrationDTO 
                 { 
                     Name = existingUser.Name,
                     Email = existingUser.Email 
                 };

                return registrationDto;
            }
            return null;
        }

        public int ResetPassword(ResetPasswordDTO resetPassword)
        {
            var ValidEmail = _helper.VerifyPassword(resetPassword.Email, resetPassword.Code);
            if(ValidEmail)
            {
                var VerifyOldPassword = _helper.VerifyPassword(resetPassword.OldPassword, _postgreContext.registrations.FirstOrDefault(r => r.Email == resetPassword.Email).Password);
                var existingUser = _postgreContext.registrations.FirstOrDefault(r => r.Email == resetPassword.Email && VerifyOldPassword);
                if (existingUser != null)
                {
                    existingUser.Password = _helper.HashPassword(resetPassword.NewPassword != null ? resetPassword.NewPassword : string.Empty);
                    _postgreContext.SaveChanges();
                    return 1; // Password reset successful
                }
                return 0; // User not found
            }
            return -1; // Invalid email or code
        }

        public DashboardResponse GetDashboardData(int userId)
        {
            var TotalSpent = _postgreContext.expense.Where(e => e.UserId == userId && e.ExpenseType == "expense").Sum(e => e.Amount);
            var TotalIncome = _postgreContext.expense.Where(e => e.UserId == userId && e.ExpenseType == "income").Sum(e => e.Amount);
            var NetSaving = TotalIncome - TotalSpent;
            var NoofTransactions = _postgreContext.expense.Where(e => e.UserId == userId && e.ExpenseType == "expense").Count();
            var Credits = _postgreContext.expense.Where(e => e.UserId == userId && e.ExpenseType == "income").Count();
            var status = (TotalIncome >= TotalSpent) ? 'P' : 'N';
            var dashboardResponse = new DashboardResponse
            {
                TotalSpent = TotalSpent,
                TotalIncome = TotalIncome,
                NetSaving = NetSaving,
                NoofTransactions = NoofTransactions,
                Credits = Convert.ToInt32(Credits),
                Status = status,
                Transaction = _postgreContext.expense.Where(e => e.UserId == userId).Select(e => new TransactionResponseDto
                {
                    Amount = e.Amount,
                    Date = e.DateofExpense,
                    Category = e.Category,
                    Type = e.ExpenseType == "income" ? 'C' : 'D',
                    Description = e.Description,
                    Type2 = e.ExpenseType == "income" ? "income" : "expense"
                }).ToList()
            };
            return dashboardResponse;
        }
    }
}
