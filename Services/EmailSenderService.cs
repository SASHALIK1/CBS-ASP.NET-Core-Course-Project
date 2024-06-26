﻿using System.Net.Mail;
using System.Net;
using CBS_ASP.NET_Core_Course_Project.Models;
using System.Reflection;
using System.Text;

namespace CBS_ASP.NET_Core_Course_Project.Services
{
    //public interface IEmailSender
    //{
    //    Task SendEmailAsync(string recipientEmail, string subject, string body);
    //}
    public class EmailSenderService// : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ExchangeRateService _exchangeRateService;
        public EmailSenderService(IConfiguration configuration, ExchangeRateService exchangeRateService)
        {

            _emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            _exchangeRateService = exchangeRateService;
        }
        public async Task SendDailyExchangeRateEmailAsync()
        {
            string subject = "Денний курс валют";
            string body;

            List<string> recipientEmails = Database.GetEmailsWithSendEmailsOn();

            List<BankRates> banksRates = await _exchangeRateService.GetAllExchangeRates();

            BankRates nbuRates = new BankRates("NBU");
            nbuRates.rates.Add(await _exchangeRateService.GetNBUExchangeRateAsync("usd"));
            nbuRates.rates.Add(await _exchangeRateService.GetNBUExchangeRateAsync("eur"));

            StringBuilder stringbuilder = new StringBuilder();
            stringbuilder.AppendLine("\nСьогоднішні курси валют:");
            foreach (string recipientEmail in recipientEmails)
            {

                foreach (ExchangeRate exchangeRates in nbuRates.rates)
                {
                    stringbuilder.AppendLine("\nКурс НБУ");
                    stringbuilder.AppendLine("\n" + exchangeRates.buyRate.ToString());
                }

                foreach (BankRates bank in banksRates)
                {
                    foreach (ExchangeRate exchangeRate in bank.rates)
                    {
                        stringbuilder.AppendLine($"\n{exchangeRate.currency}: {exchangeRate.buyRate} / {exchangeRate.sellRate}");
                    }
                }
            }

            body = stringbuilder.ToString();
            foreach (string recipientEmail in recipientEmails)
            {
                await SendEmailAsync(recipientEmail, subject, body);
            }
        }
        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_emailConfig.SmtpServer)
            {
                Port = _emailConfig.Port,
                Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password),
                EnableSsl = _emailConfig.EnableSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.UserName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(recipientEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
