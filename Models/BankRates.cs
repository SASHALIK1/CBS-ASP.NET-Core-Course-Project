namespace CBS_ASP.NET_Core_Course_Project.Models
{
    public class BankRates
    {
        public string bankName { get; set; }
        public List<ExchangeRate> rates { get; set; }
        public BankRates(string bankName)
        {
            rates = new List<ExchangeRate>();
            this.bankName = bankName;
        }
    }
}
