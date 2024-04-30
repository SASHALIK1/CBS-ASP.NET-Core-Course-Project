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
            BankRates NBURates = new BankRates("NBU");
            NBURates.rates.Add(await _exchangeRateService.GetNBUExchangeRateAsync("usd"));
            NBURates.rates.Add(await _exchangeRateService.GetNBUExchangeRateAsync("eur"));

            BankRates monobankRates = new BankRates("Monobank");
            monobankRates.rates.Add(await _exchangeRateService.GetMonobankExchangeRateAsync("usd"));
            monobankRates.rates.Add(await _exchangeRateService.GetMonobankExchangeRateAsync("eur"));

            BankRates privatRates = new BankRates("Privatbank");
            privatRates.rates.Add(await _exchangeRateService.GetPrivatBankExchangeRateAsync("usd"));
            privatRates.rates.Add(await _exchangeRateService.GetPrivatBankExchangeRateAsync("eur"));

            BankRates OschadRates = new BankRates("OschadBank");
            OschadRates.rates.Add(await _exchangeRateService.GetOschadBankExchangeRateAsync("usd"));
            OschadRates.rates.Add(await _exchangeRateService.GetOschadBankExchangeRateAsync("eur"));

            BankRates pumbRates = new BankRates("PUMB");
            pumbRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("usd", "pumb"));
            pumbRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("eur", "pumb"));

            BankRates otpRates = new BankRates("Otpbank");
            otpRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("usd", "otp-bank"));
            otpRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("eur", "otp-bank"));

            BankRates aBankRates = new BankRates("A-Bank");
            aBankRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("usd", "a-bank"));
            aBankRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("eur", "a-bank"));

            BankRates iziBankRates = new BankRates("Izibank");
            iziBankRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("usd", "izibank"));
            iziBankRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("eur", "izibank"));
            
            BankRates sensebankRates = new BankRates("sensebank");
            sensebankRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("usd", "sensebank"));
            sensebankRates.rates.Add(await _exchangeRateService.GetBankExchangeRateAsync("eur", "sensebank"));

            List<BankRates> _banks = new List<BankRates>();
            _banks.Add(monobankRates);
            _banks.Add(privatRates);
            _banks.Add(OschadRates);
            _banks.Add(pumbRates);
            _banks.Add(otpRates);
            _banks.Add(aBankRates);
            _banks.Add(iziBankRates);
            _banks.Add(sensebankRates);
            return View(new ExchangeRatesViewModel
            {
                nbuRates = NBURates,
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
