using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StajyerProject.Core.DTO;
using StajyerProject.Service;

namespace StajyerProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly CrudService _crudService;

        public CrudController(IConfiguration configuration, CrudService crudService)
        {
            _configuration = configuration;
            _crudService = crudService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMesaj([FromBody] MesajRequest request)
        {
            if (request == null)
                return BadRequest("Mesaj verisi boş gönderildi.");

            var result = await _crudService.CreateMesajAsync(request);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result);



        }

        [HttpPost("ekle")]
        public async Task<IActionResult> AddMesaj([FromBody] MesajResponse model)
        {
            try
            {
                int insertedId = await _crudService.AddMesajAsync(model);
                return Ok(insertedId); // sadece sayı döner, örnek: 5
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata oluştu: {ex.Message}");
            }
        }



        [HttpPost("ef-create")]
        public async Task<IActionResult> EfCreate([FromBody] MesajRequest request)
        {
            var result = await _crudService.CreateWithSpEfAsync(request);
            return result.Success ? Ok(result) : StatusCode(500, result);
        }

        [HttpPost("dapper-create")]
        public async Task<IActionResult> DapperCreate([FromBody] MesajRequest request)
        {
            var result = await _crudService.CreateWithSpDapperAsync(request);
            return result.Success ? Ok(result) : StatusCode(500, result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMesaj([FromBody] MesajRequest request)
        {
            if (request == null || request.id <= 0)
                return BadRequest("Geçersiz veri.");

            var success = await _crudService.UpdateMesajAsync(request);

            if (success)
                return Ok(new { success = true, message = "Güncelleme başarılı." });

            return StatusCode(500, new { success = false, message = "Güncelleme başarısız." });
        }

        [HttpPost("create-ado")]
        public async Task<IActionResult> AddMesajAdo([FromBody] MesajRequest request)
        {
            if (request == null)
                return BadRequest("İstek verisi boş.");

            var result = await _crudService.AddMesajAsyncAdoNet(request);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        [HttpPut("update-ef")]
        public async Task<IActionResult> UpdateMesajEf([FromBody] MesajRequest request)
        {
            if (request == null || request.id <= 0)
                return BadRequest("Geçersiz veri.");

            var result = await _crudService.UpdateMesajEfAsync(request);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

    }
}
