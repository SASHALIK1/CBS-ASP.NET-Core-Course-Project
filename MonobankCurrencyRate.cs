namespace CBS_ASP.NET_Core_Course_Project
{
    public class MonobankCurrencyRate
    {
        public int currencyCodeA { get; set; }
        public int currencyCodeB { get; set; }
        public long date { get; set; }
        public float rateSell { get; set; }
        public float rateBuy { get; set; }
        public float rateCross { get; set; }
    }
}
