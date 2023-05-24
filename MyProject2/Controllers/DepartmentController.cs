using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject2.Models;
using MyProject2.Repository;
using MyProject2.Repository.Interface;
using System.Net;

namespace MyProject2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentRepository departmentRepository;
        private int ID;

        public DepartmentController(DepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        [HttpPost]
        public ActionResult Insert(Department department)
        {
            int result = departmentRepository.Insert(department);
            if (result == 1)
            {
                var get = departmentRepository.Get(ID);
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan", data = get });
            }
            else
            {
                return StatusCode(500, new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "ID yang anda masukkan sudah terdaftar."
                });

            }
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var get = await departmentRepository.Get();
            if (get != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan", data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan", data = get });
            }
        }

        [HttpGet("{ID}")]
        public ActionResult Get(int ID)
        {
            var get = departmentRepository.Get(ID);
            if (get == null)
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan", data = get });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan", data = get });
        }

        [HttpPut]
        public ActionResult Update(Department department)
        {
            var get = departmentRepository.Get(ID);

            departmentRepository.Update(department);
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil diubah", data = get });

        }

        [HttpDelete("{ID}")]
        public ActionResult Delete(int ID)
        {
            var get = departmentRepository.Delete(ID);
            if (get == null)
            {
                

                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan", data = get });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data berhasil dihapus", data = get });
        }
    }
}
