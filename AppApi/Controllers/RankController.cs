using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private IRanksRepositories _rankRepository;
        public RankController(IRanksRepositories rankRepository)
        {
            _rankRepository = rankRepository;
        }

        // GET: api/<RankController>
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            var a = await _rankRepository.GetAll();
            return Ok(a);
        }

        // GET api/<RankController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _rankRepository.GetById(id);
            return Ok(a);
        }

        //// POST api/<RankController>
        //[HttpPost("add")]
        //public async Task<IActionResult> Post(Blog obj)
        //{
        //    var a = await _blogRepository.Add(obj);
        //    return Ok(a);
        //}

        // PUT api/<RankController>/5
        //[HttpPut("update")]
        //public async Task<IActionResult> Put(Blog obj)
        //{
        //    var a = await _blogRepository.Update(obj);
        //    return Ok(a);
        //}

        // DELETE api/<RankController>/5
        //[HttpDelete("delete/{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    await _blogRepository.Delete(id);
        //    return Ok();
        //}
    }
}
