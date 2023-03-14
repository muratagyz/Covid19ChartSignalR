using Covid19ChartSignalR.API.Models;
using Covid19ChartSignalR.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Covid19ChartSignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SaveCovid(covid);
            //var covids = await _covidService.GetList().ToListAsync();

            return Ok(_covidService.GetCovidList());
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random rnd = new Random();
            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                foreach (var item in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid
                    { City = (ECity)item, Count = rnd.Next(100, 1000), CovidDate = DateTime.Now.AddDays(x) };
                    _covidService.SaveCovid(newCovid).Wait();
                    System.Threading.Thread.Sleep(1000);
                }
            });
            return Ok("Covid 19 dataları veri tabanına kaydedildi");
        }
    }
}
