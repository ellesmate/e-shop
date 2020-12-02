using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shop.Application.Emails;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.UI.Workers.Email
{
    public class EmailService : BackgroundService
    {
        private const int maxAttempts = 3;

        private readonly IOptionsMonitor<EmailSettings> _optionsMonitor;
        private readonly ILogger<EmailService> _logger;
        private readonly IEmailQueue _emailQueue;
        private readonly IEmailSink _emailSink;

        public EmailService(
            IOptionsMonitor<EmailSettings> optionsMonitor,
            ILogger<EmailService> logger,
            IEmailQueue emailQueue,
            IEmailSink emailSink)
        {
            _optionsMonitor = optionsMonitor;
            _logger = logger;
            _emailQueue = emailQueue;
            _emailSink = emailSink;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SendEmailRequest request = null;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    request = await _emailQueue.ReadAsync();

                    _logger.LogInformation("Sending Email to {0} , with subject {1}", request.To, request.Subject);

                    var settings = _optionsMonitor.CurrentValue;
                    using var smtp = CreateClient(settings);
                    using var mailMessage = CreateMessage(settings, request);
                    await smtp.SendMailAsync(mailMessage);
                }
                catch (SmtpException e)
                {
                    if (request?.Failed < maxAttempts)
                    {
                        request.Failed++;
                        await _emailSink.SendAsync(request);
                    }
                    
                    _logger.LogError(e, e.Message);
                    _logger.LogError(e, "Failed to send email");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    _logger.LogError(e, "Failed to send email");
                }
            }
        }

        private static SmtpClient CreateClient(EmailSettings settings)
        {
            return new SmtpClient
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Account, settings.Password),
                Host = settings.Host,
                Port = int.Parse(settings.Port),
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };
        }

        private static MailMessage CreateMessage(EmailSettings settings, SendEmailRequest request)
        {
            return new MailMessage(settings.SenderEmail, request.To)
            {
                Subject = request.Subject,
                Body = request.Message,
                IsBodyHtml = request.Html,
            };
        }
    }
}
