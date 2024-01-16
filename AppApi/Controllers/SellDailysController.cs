using AppApi.ViewModels.SelldaillysViewModels;
using AppData.IRepositories;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace AppApi.Controllers
{
    [Route("api/SellDaillys")]
    [ApiController]
    public class SellDailysController : ControllerBase
    {
        private ISellDailyRepository _sellDailyRepository;

        public SellDailysController(ISellDailyRepository selldailyRepository)
        {
            _sellDailyRepository = selldailyRepository;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _sellDailyRepository.GetAll();
            return Ok(a);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _sellDailyRepository.GetById(id);
            return Ok(a);
        }

        [HttpGet("getYearId/{year}")]
        public async Task<IActionResult> GetByYear(int year)
        {
            var results = new SellMonthllysViewModel();
            var datas = await _sellDailyRepository.GetByYear(year);
            List<string> date = new List<string>();
            List<decimal?> data_money = new List<decimal?>();
            List<decimal?> data_quantity = new List<decimal?>();
            for (int i = 1; i <= 12; i++)
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

        [HttpGet("getMonthId/{month}")]
        public async Task<IActionResult> GetByMonth(int month)
        {
            var results = new SellMonthllysViewModel();
            var datas = await _sellDailyRepository.GetByMonth(month);
            DateTime now = DateTime.Now;
            int day = now.Day;
            List<string> date = new List<string>();
            List<decimal?> data_money = new List<decimal?>();
            List<decimal?> data_quantity = new List<decimal?>();
      
            for (int i = day - 7; i <= day ; i++)
            {
                var dataMonth = datas.Where(e => e.CreateTime.Day == i);
                var time = $"{i}/{month}";
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
