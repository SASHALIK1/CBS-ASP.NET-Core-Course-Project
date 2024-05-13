using System;
using System.Globalization;
using System.Net;
using System.Text.Json;
using CBS_ASP.NET_Core_Course_Project.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;

namespace CBS_ASP.NET_Core_Course_Project.Services
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const int cacheSaveMinutes = 5;

        public ExchangeRateService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<BankRates>> GetAllExchangeRates()
        {
            BankRates monobankRates = new BankRates("Monobank");
            monobankRates.rates.Add(await GetMonobankExchangeRateAsync("usd"));
            monobankRates.rates.Add(await GetMonobankExchangeRateAsync("eur"));

            BankRates privatRates = new BankRates("Privatbank");
            privatRates.rates.Add(await GetPrivatBankExchangeRateAsync("usd"));
            privatRates.rates.Add(await GetPrivatBankExchangeRateAsync("eur"));

            BankRates OschadRates = new BankRates("OschadBank");
            OschadRates.rates.Add(await GetOschadBankExchangeRateAsync("usd"));
            OschadRates.rates.Add(await GetOschadBankExchangeRateAsync("eur"));

            BankRates pumbRates = new BankRates("PUMB");
            pumbRates.rates.Add(await GetBankExchangeRateAsync("usd", "pumb"));
            pumbRates.rates.Add(await GetBankExchangeRateAsync("eur", "pumb"));

            BankRates otpRates = new BankRates("Otpbank");
            otpRates.rates.Add(await GetBankExchangeRateAsync("usd", "otp-bank"));
            otpRates.rates.Add(await GetBankExchangeRateAsync("eur", "otp-bank"));

            BankRates aBankRates = new BankRates("A-Bank");
            aBankRates.rates.Add(await GetBankExchangeRateAsync("usd", "a-bank"));
            aBankRates.rates.Add(await GetBankExchangeRateAsync("eur", "a-bank"));

            //BankRates iziBankRates = new BankRates("Izibank");
            //iziBankRates.rates.Add(await GetBankExchangeRateAsync("usd", "izibank"));
            //iziBankRates.rates.Add(await GetBankExchangeRateAsync("eur", "izibank"));

            BankRates sensebankRates = new BankRates("sensebank");
            sensebankRates.rates.Add(await GetBankExchangeRateAsync("usd", "sensebank"));
            sensebankRates.rates.Add(await GetBankExchangeRateAsync("eur", "sensebank"));

            List<BankRates> banks = new List<BankRates>();
            banks.Add(monobankRates);
            banks.Add(privatRates);
            banks.Add(OschadRates);
            banks.Add(pumbRates);
            banks.Add(otpRates);
            banks.Add(aBankRates);
            //banks.Add(iziBankRates);
            banks.Add(sensebankRates);

            return banks;
        }

        public async Task<ExchangeRate> GetMonobankExchangeRateAsync(string currencyName)
        {
            const string cacheKey = "MonobankAPI";
            string responseString;
            if (!_cache.TryGetValue(cacheKey, out string cachedResponseString))
            {
                responseString = await _httpClient.GetStringAsync("https://api.monobank.ua/bank/currency");
                _cache.Set(cacheKey, responseString, TimeSpan.FromMinutes(cacheSaveMinutes));
            }
            else
            {
                responseString = (string)cachedResponseString;
            }
            List<MonobankCurrencyRate> deserializedString = JsonSerializer.Deserialize<List<MonobankCurrencyRate>>(responseString);
            foreach (MonobankCurrencyRate currency in deserializedString)
            {
                if (currencyName == "usd")
                {
                    if (currency.currencyCodeA == 840 && currency.currencyCodeB == 980)
                    {
                        return new ExchangeRate("usd", currency.rateBuy, currency.rateSell);
                    }
                }
                else if (currencyName == "eur")
                {
                    if (currency.currencyCodeA == 978 && currency.currencyCodeB == 980)
                    {
                        return new ExchangeRate("eur", currency.rateBuy, currency.rateSell);
                    }
                }
            }
            return new ExchangeRate("usd", 5, 5);
        }


        public async Task<ExchangeRate> GetPrivatBankExchangeRateAsync(string currencyName)
        {
            const string cacheKey = "PrivatbankAPI";
            string responseString;
            if (!_cache.TryGetValue(cacheKey, out string cachedResponseString))
            {
                responseString = await _httpClient.GetStringAsync("https://api.privatbank.ua/p24api/pubinfo?exchange&json&coursid=11");
                _cache.Set(cacheKey, responseString, TimeSpan.FromMinutes(cacheSaveMinutes));
            }
            else
            {
                responseString = (string)cachedResponseString;
            }
            List<PrivatbankCurrencyRate> deserializedString = JsonSerializer.Deserialize<List<PrivatbankCurrencyRate>>(responseString);
            ExchangeRate exchangeRate = new ExchangeRate();
            
            foreach (PrivatbankCurrencyRate currency in deserializedString)
            {
                if (currency.ccy == currencyName.ToUpper())
                {
                    return new ExchangeRate(currencyName, float.Parse(currency.buy.Replace('.', ',')), float.Parse(currency.sale.Replace('.', ',')));
                }
            }
            return new ExchangeRate("usd", 2, 2);
        }
        public async Task<ExchangeRate> GetNBUExchangeRateAsync(string currencyName)
        {
            const string cacheKey = "nbuAPI";
            string responseString;
            if (!_cache.TryGetValue(cacheKey, out string cachedResponseString))
            {
                responseString = await _httpClient.GetStringAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
                _cache.Set(cacheKey, responseString, TimeSpan.FromMinutes(cacheSaveMinutes));
            }
            else
            {
                responseString = (string)cachedResponseString;
            }
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
        public async Task<ExchangeRate> GetOschadBankExchangeRateAsync(string currencyName)
        {
            const string cacheKey = "OschadbankAPI";
            string responseString;
            if (!_cache.TryGetValue(cacheKey, out string cachedResponseString))
            {
                responseString = await _httpClient.GetStringAsync("https://www.oschadbank.ua/");
                _cache.Set(cacheKey, responseString, TimeSpan.FromMinutes(cacheSaveMinutes));
            }
            else
            {
                responseString = (string)cachedResponseString;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(responseString);

            var eurItem = doc.DocumentNode.SelectSingleNode("//div[@class='currency__item']/span[@class='currency__item_name'][text()='EUR']/..");
            var usdItem = doc.DocumentNode.SelectSingleNode("//div[@class='currency__item']/span[@class='currency__item_name'][text()='USD']/..");

            if (currencyName == "usd")
            {
                var usdBuyRate = usdItem.SelectSingleNode(".//span[@class='currency__item_value'][1]/span").InnerText;
                var usdSellRate = usdItem.SelectSingleNode(".//span[@class='currency__item_value'][2]/span").InnerText;
                return new ExchangeRate("usd", float.Parse(usdBuyRate.Replace('.', ',')), float.Parse(usdSellRate.Replace('.', ',')));
            }
            else if (currencyName == "eur")
            {
                var eurBuyRate = eurItem.SelectSingleNode(".//span[@class='currency__item_value'][1]/span").InnerText;
                var eurSellRate = eurItem.SelectSingleNode(".//span[@class='currency__item_value'][2]/span").InnerText;
                return new ExchangeRate("eur", float.Parse(eurBuyRate.Replace('.', ',')), float.Parse(eurSellRate.Replace('.', ',')));
            }
            return new ExchangeRate("usd", 5, 40);
        }

        public async Task<ExchangeRate> GetBankExchangeRateAsync(string currencyName, string bankName)
        {
            const string cacheKeyUSD = "minfinUsdAPI";
            const string cacheKeyEUR = "minfinEurAPI";

            string usdUrl = "https://minfin.com.ua/ua/currency/banks/eur/";
            string eurUrl = "https://minfin.com.ua/ua/currency/banks/usd/";

            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";

            string responseString;

            HtmlDocument doc = new HtmlDocument();
            if (currencyName == "usd")
            {
                if (!_cache.TryGetValue(cacheKeyUSD, out string cachedResponseString))
                {
                    HttpClient client = new HttpClient();

                    client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                    HttpResponseMessage response = await client.GetAsync(usdUrl);

                    responseString = await response.Content.ReadAsStringAsync();

                    _cache.Set(cacheKeyUSD, responseString, TimeSpan.FromMinutes(cacheSaveMinutes));
                }
                else
                {
                    responseString = (string)cachedResponseString;
                }
            }
            else //eur
            {
                if (!_cache.TryGetValue(cacheKeyEUR, out string cachedResponseString))
                {
                    HttpClient client = new HttpClient();

                    client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                    HttpResponseMessage response = await client.GetAsync(eurUrl);

                    responseString = await response.Content.ReadAsStringAsync();

                    _cache.Set(cacheKeyEUR, responseString, TimeSpan.FromMinutes(cacheSaveMinutes));
                }
                else
                {
                    responseString = (string)cachedResponseString;
                }
            }
            
            doc.LoadHtml(responseString);

            HtmlNode row = doc.DocumentNode.SelectSingleNode($"//td[contains(@class, 'js-ex-rates') and contains(@class, 'mfcur-table-bankname') and contains(a/@href, '{bankName}')]");

            HtmlNode buyRateNode = row.SelectSingleNode(".//following-sibling::td[contains(@class, 'mfm-text-right')]");
            string buyRate = buyRateNode.InnerText.Trim();

            HtmlNode sellRateNode = row.SelectSingleNode(".//following-sibling::td[contains(@class, 'mfm-text-left')]");
            string sellRate = sellRateNode.InnerText.Trim();

            return new ExchangeRate(currencyName, float.Parse(buyRate.Replace('.', ',')), float.Parse(sellRate.Replace('.', ',')));
        }
    }
}
