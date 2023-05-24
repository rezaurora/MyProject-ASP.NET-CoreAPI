using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject2.Models;
using MyProject2.Repository;
using MyProject2.Repository.Interface;
using MyProject2.ViewModels;
using System.Net;

namespace MyProject2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository employeeRepository;
        private string NIK;
        public EmployeeController(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        public ActionResult Insert(Employee employee)
        {
            int result = employeeRepository.Insert(employee);
            if (result == 1)
            {
                var get = employeeRepository.Get();
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan", data = get });
            }
            else
            {
                return StatusCode(500, new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Nomor HP atau Email yang anda masukkan sudah terdaftar. " +
                    "Silahkan gunakan nomor HP atau email yang lain"
                });

            }
        }

        /*[HttpPost("Register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            int result = employeeRepository.Register(registerVM);
            if (result != 0)
            {
                //var get = accountRepository.Get();
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan", data = registerVM });
            }

            return StatusCode(500, new
            {
                status = HttpStatusCode.BadRequest,
                message = "NIK yang anda masukkan salah."
            });
        }*/

        [HttpGet]
        public ActionResult Get()
        {
            var get = employeeRepository.Get();
            if (get != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil didapatkan", data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan", data = get });
            }
        }

        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            var get = employeeRepository.Get(NIK);
            if (get == null)
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan", data = get });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan", data = get });
        }

        [HttpGet("/newObjext")]
        public ActionResult GetSeveralRows()
        {
            var myEmp = employeeRepository.GetRows();
            if(myEmp != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = employeeRepository.Get().Count() + " Data ditemukan", Data = myEmp });
            }
            return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan", Data = myEmp });
        }

        [HttpGet("TestCORS")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS berhasil");
        }

        [HttpPut]
        public ActionResult Update(Employee employee)
        {
            var get = employeeRepository.Get(NIK);

            employeeRepository.Update(employee);
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil diubah", data = get });

        }

        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            var get = employeeRepository.Get(NIK);
            if (get != null)
            {
                employeeRepository.Delete(NIK);
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil dihapus", data = get });
            }
            return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan", data = get });
        }
    }
}
