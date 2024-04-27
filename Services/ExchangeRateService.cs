using System.Globalization;
using System.Text.Json;

namespace CBS_ASP.NET_Core_Course_Project.Services
{
    //public interface IExchangeRateService
    //{
    //    Task<ExchangeRate> GetMonobankExchangeRateAsync();
    //    Task<ExchangeRate> GetPrivatBankExchangeRateAsync();
    //    Task<ExchangeRate> GetNBUExchangeRateAsync();
    //}
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExchangeRate> GetMonobankExchangeRateAsync(string currencyName)
        {
            string responseString = await _httpClient.GetStringAsync("https://api.monobank.ua/bank/currency");
            List<MonobankCurrencyRate> deserializedString = JsonSerializer.Deserialize<List<MonobankCurrencyRate>>(responseString);

            foreach (MonobankCurrencyRate currency in deserializedString)
            {
                if (currencyName == "usd")
                {
                    if (currency.currencyCodeA == 840 && currency.currencyCodeB == 980)
                    {
                        Console.WriteLine(currency.currencyCodeA);
                        return new ExchangeRate("usd", currency.rateBuy, currency.rateSell);
                    }
                }
                else if (currencyName == "eur")
                {
                    if (currency.currencyCodeA == 978 && currency.currencyCodeB == 980)
                    {
                        Console.WriteLine(currency.currencyCodeA);
                        return new ExchangeRate("eur", currency.rateBuy, currency.rateSell);
                    }
                }
            }


            return new ExchangeRate("usd", 5, 5);
        }


        public async Task<ExchangeRate> GetPrivatBankExchangeRateAsync(string currencyName)
        {
            string responseString = await _httpClient.GetStringAsync("https://api.privatbank.ua/p24api/pubinfo?exchange&json&coursid=11");
            List<PrivatbankCurrencyRate> deserializedString = JsonSerializer.Deserialize<List<PrivatbankCurrencyRate>>(responseString);


            foreach (PrivatbankCurrencyRate currency in deserializedString)
            {
                if (currency.ccy == currencyName.ToUpper())
                {
                    return new ExchangeRate(currencyName, float.Parse(currency.buy.Replace('.', ',')), float.Parse(currency.sale.Replace('.', ',')));
                }
            }

            return new ExchangeRate("usd", 5, 5);
        }

        public async Task<ExchangeRate> GetNBUExchangeRateAsync(string currencyName)
        {
            string responseString = await _httpClient.GetStringAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
            List<NBUExchangeRate> deserializedString = JsonSerializer.Deserialize<List<NBUExchangeRate>>(responseString);

            foreach (NBUExchangeRate currency in deserializedString)
            {
                if (currency.cc == currencyName.ToUpper())
                {
                    return new ExchangeRate(currencyName, currency.rate, currency.rate);
                }
            }

            return new ExchangeRate("usd", 39, 40);
        }
    }
}
