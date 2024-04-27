using CBS_ASP.NET_Core_Course_Project.Models;
using CBS_ASP.NET_Core_Course_Project.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CurrencyExchangeRates()
        {
            BankRates monobankRates = new BankRates("monobank");
            monobankRates.rates.Add(await _exchangeRateService.GetMonobankExchangeRateAsync("usd"));
            monobankRates.rates.Add(await _exchangeRateService.GetMonobankExchangeRateAsync("eur"));

            BankRates privatRates = new BankRates("privatbank");
            privatRates.rates.Add(await _exchangeRateService.GetPrivatBankExchangeRateAsync("usd"));
            privatRates.rates.Add(await _exchangeRateService.GetPrivatBankExchangeRateAsync("eur"));

            BankRates NBURates = new BankRates("NBU");
            NBURates.rates.Add(await _exchangeRateService.GetNBUExchangeRateAsync("usd"));
            NBURates.rates.Add(await _exchangeRateService.GetNBUExchangeRateAsync("eur"));
            //var model = new ExchangeRatesViewModel();

            List<BankRates> _banks = new List<BankRates>();
            _banks.Add(monobankRates);
            _banks.Add(privatRates);
            _banks.Add(NBURates);
            return View(new ExchangeRatesViewModel
            {
                banks = _banks
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
