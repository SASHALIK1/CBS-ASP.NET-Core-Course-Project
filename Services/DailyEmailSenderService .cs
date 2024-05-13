namespace CBS_ASP.NET_Core_Course_Project.Services
{
    public class DailyEmailSenderService : BackgroundService
    {
        private readonly EmailSenderService _emailSender;

        public DailyEmailSenderService(EmailSenderService emailSender)
        {
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;
                DateTime scheduledTime = new DateTime(now.Year, now.Month, now.Day, 14, 30, 0);

                if (now > scheduledTime)
                {
                    scheduledTime = scheduledTime.AddDays(1);
                }

                TimeSpan delay = scheduledTime - now;

                await Task.Delay(delay, stoppingToken);

                await _emailSender.SendDailyExchangeRateEmailAsync();
            }
        }
    }
}
