using CBS_ASP.NET_Core_Course_Project.Models;
using CBS_ASP.NET_Core_Course_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;

namespace CBS_ASP.NET_Core_Course_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExchangeRateService _exchangeRateService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ExchangeRateService exchangeRateService, ILogger<HomeController> logger)
        {
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.sendEmails = Database.GetSendEmailsStatusByEmail(HttpContext.User.Identity.Name);
            return View();
        }

        public async Task<IActionResult> CurrencyExchangeRates()
        {
            ViewBag.sendEmails = Database.GetSendEmailsStatusByEmail(HttpContext.User.Identity.Name);

            BankRates NBURates = new BankRates("NBU");
            NBURates.rates.Add(await _exchangeRateService.GetNBUExchangeRateAsync("usd"));
            NBURates.rates.Add(await _exchangeRateService.GetNBUExchangeRateAsync("eur"));

            return View(new ExchangeRatesViewModel(NBURates, (await _exchangeRateService.GetAllExchangeRates())));
        }

        public IActionResult Privacy()
        {
            ViewBag.sendEmails = Database.GetSendEmailsStatusByEmail(HttpContext.User.Identity.Name);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
