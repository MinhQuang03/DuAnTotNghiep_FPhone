using AppApi.ViewModels.SelldaillysViewModels;
using AppData.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace AppApi.Controllers
{
    [Route("api/SellMonthlys")]
    [ApiController]
    public class SellMonthlysController : ControllerBase
    {
        private ISellMonthlyRepository _sell;

        public SellMonthlysController(ISellMonthlyRepository sellmonthlyRepository)
        {
            _sell = sellmonthlyRepository;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _sell.GetAll();
            return Ok(a);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _sell.GetById(id);
            return Ok(a);
        }

        [HttpGet("getYearId/{year}")]
        public async Task<IActionResult> GetByYear(int year)
        {
            var results = new SellMonthllysViewModel();
            var datas = await _sell.GetByYear(year);
            DateTime now = DateTime.Now;
            int y = now.Year;
            List<string> date = new List<string>();
            List<decimal?> data_money = new List<decimal?>();
            List<decimal?> data_quantity = new List<decimal?>();
            for (int i = 1 ; i <= 12; i++)
            {
                var dataMonth = datas.Where(e => e.CreateTime.Month == i);
                var time = $"{i}/{year}";
                date.Add(time);
                data_money.Add(dataMonth.Sum(e => e.TotalMoneys));
                data_quantity.Add(dataMonth.Sum(e => e.TotalQuantity));
            }
            results.Date = date;
            results.DataQuantiy = data_quantity;
            results.DataMoney = data_money;

            return Ok(results);
        }
    }
}
