namespace CBS_ASP.NET_Core_Course_Project
{

    public class ExchangeRate
    {
        private string _currency;
        private float _buyRate;
        private float _sellRate;

        public string currency 
        { 
            get { return _currency; }
            set {  _currency = value.ToUpper(); }
        }
        public float buyRate
        {
            get { return _buyRate; }
            set { _buyRate = float.Parse(value.ToString("0.00").Replace('.', ',')); }
        }

        public float sellRate
        {
            get { return _sellRate; }
            set { _sellRate = float.Parse(value.ToString("0.00").Replace('.', ',')); }
        }
        public ExchangeRate(string currency, float buyRate, float sellRate)
        {
            this.currency = currency;
            this.buyRate = buyRate;
            this.sellRate = sellRate;
        }
        public ExchangeRate()
        {

        }

    }
}
