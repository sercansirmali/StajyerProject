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
            _crudService = _crudService;
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
    }
}
