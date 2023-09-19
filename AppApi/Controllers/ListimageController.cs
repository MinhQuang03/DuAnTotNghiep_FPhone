using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListimageController : ControllerBase
    {
        private IListImgaeRepository _ListImgaeRepository;
        public ListimageController(IListImgaeRepository listImgaeRepository)
        {
            _ListImgaeRepository = listImgaeRepository;
        }


        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _ListImgaeRepository.GetAll();
            return Ok(a);
        }
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _ListImgaeRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ProductionCompanyController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(ListImage obj)
        {
            var a = await _ListImgaeRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ProductionCompanyController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(ListImage obj)
        {
            var a = await _ListImgaeRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ProductionCompanyController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _ListImgaeRepository.Delete(id);
            return Ok();
        }
    }
}
