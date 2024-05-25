using Calendar.Attributes;
using Calendar.BL.Services.HolidayService;
using Calendar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Calendar.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private readonly IHolidayService _holidayService;

        public HomeController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetSwedishHolidays()
        {
            var swedishHolidays = await _holidayService.GetSwedishHolidaysFromAPI();
            return Json(swedishHolidays);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
