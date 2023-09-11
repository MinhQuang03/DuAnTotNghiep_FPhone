using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimController : ControllerBase
    {
        private ISimRepository _simRepository;

        public SimController(ISimRepository simRepository)
        {
            _simRepository = simRepository;
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Sim sim)
        {
            var result = await  _simRepository.Create(sim);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest("You don't have access"); 
        }
    }
}
