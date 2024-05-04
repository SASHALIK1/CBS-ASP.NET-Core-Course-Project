namespace CBS_ASP.NET_Core_Course_Project.Models
{
    public class ExchangeRatesViewModel
    {
        public BankRates nbuRates { get; set; }
        public List<BankRates> banks { get; set; }

        public ExchangeRatesViewModel (BankRates nbuRates, List<BankRates> banks)
        {
            this.nbuRates = nbuRates;
            this.banks = banks;
        }
    }
}
