using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyProject2.Models;
using MyProject2.Repository;
using MyProject2.Repository.Interface;
using MyProject2.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MyProject2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountRepository accountRepository;
        private readonly EmployeeRepository employeeRepository;
        //private string Employee;
        public IConfiguration _configuration;
        public static Employee employee = new Employee();

        public AccountController(AccountRepository accountRepository, IConfiguration config)
        {
            this.accountRepository = accountRepository;
            _configuration = config;
        }


        [HttpPost("Register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            if (accountRepository.isExist(registerVM.Phone, registerVM.Email))
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Telepon atau email sudah terdaftar");
            }
            if (registerVM.Email.Contains(" ") || registerVM.Password.Contains(" "))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Email dan telepon tidak boleh mengandung spasi");
            }
            var get = accountRepository.Register(registerVM);



            if (get != 0)
            {
                //var get = accountRepository.Get();
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan", data = get });
            }
            else
            {
                return StatusCode(500, new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "NIK yang anda masukkan salah."
                });
            }
        }

        private string ProduceToken(Employee employee)
        {
            /*var tokendesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("Email",employee.Email),
                new Claim("NIK",employee.NIK),
                new Claim("FirstName",employee.FirsName),
                new Claim("LastName",employee.LastName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(tokenkey, SecurityAlgorithms.HmacSha256)
            };*/


            List<Claim> claims=new List<Claim>
            {
                new Claim("Email",employee.Email), //
                new Claim("NIK",employee.NIK),
                new Claim("FirstName",employee.FirsName),
                new Claim("LastName",employee.LastName)
            };
            
            var tokenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Appsetting:Token").Value));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(50),
                signingCredentials: new SigningCredentials(tokenkey, SecurityAlgorithms.HmacSha256));
            var tokenhandler = new JwtSecurityTokenHandler().WriteToken(token);
            

            return tokenhandler;
        }

        [HttpPost("Login"), AllowAnonymous]
        public ActionResult Login(LoginVM loginVM)
        {
            var acc = accountRepository.Get().ToList();

            if (string.IsNullOrWhiteSpace(loginVM.Email) || string.IsNullOrWhiteSpace(loginVM.Password))
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data ditemukan" });
            }
            else if (loginVM.Email.Contains(" "))
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Email tidak boleh mengandung spasi" });
            }
            else
            {
                var myEmp = acc.FirstOrDefault(e=>e.Employee.Email==loginVM.Email);
                if (myEmp != null)
                {
                    var token = ProduceToken(myEmp.Employee);
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data ditemukan", Data = token });
                }
                else
                {
                    return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan" });
                }
            }
           
            
        }

        [HttpGet]
        public ActionResult Get()
        {
            var get = accountRepository.Get();
            if (get != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan", data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan", data = get });
            }
        }
    }
}
