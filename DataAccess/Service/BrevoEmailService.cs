using DataAccess.Migrations;
using DataAccess.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BrevoEmailSender
{
    public interface IBrevoEmailService
    {
        Task<bool> SendEmailAsync(string recipientEmail, string recipientName, string subject, string htmlContent);

        Task<bool> SendEmailWithAttachmentAsync(string recipientEmail, string recipientName, string subject, string htmlContent,
            string attachmentBase64, string attachmentName);

        Task<string> GetAccountInfoAsync();
        public void AddEmailLogs(log_email logEmail);
        public List<log_email> GetEmailLogs();
    }

    // Configuration options
    public class BrevoOptions
    {
        public string ApiKey { get; set; } = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BREVO_API_KEY")) ? Environment.GetEnvironmentVariable("BREVO_API_KEY") : throw new InvalidOperationException("BREVO_API_KEY environment variable is not set.");
        public string DefaultSenderEmail { get; set; } = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEFAULT_SENDER_MAIL")) ? Environment.GetEnvironmentVariable("DEFAULT_SENDER_MAIL") : throw new InvalidOperationException("DEFAULT_SENDER_MAIL environment variable is not set.");
        public string DefaultSenderName { get; set; } = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEFAULT_SENDER_NAME")) ? Environment.GetEnvironmentVariable("DEFAULT_SENDER_NAME") : throw new InvalidOperationException("DEFAULT_SENDER_NAME environment variable is not set.");
    }

    // Email service implementation
    public class BrevoEmailService : IBrevoEmailService
    {
        private readonly DbPostgreContext _dbPostgreContext;
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.brevo.com/v3";
        private readonly BrevoOptions _options2;

        public BrevoEmailService(HttpClient httpClient, BrevoOptions options,DbPostgreContext dbPostgreContext, BrevoOptions options2)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("api-key", options.ApiKey);
            _dbPostgreContext = dbPostgreContext;
            _options2 = options;
        }

        public async Task<bool> SendEmailAsync(
            string recipientEmail, string recipientName, string subject, string htmlContent)
        {

            try
            {
                var senderEmail = _options2.DefaultSenderEmail;
                var senderName = _options2.DefaultSenderName;
                var emailData = new
                {
                    sender = new { email = senderEmail, name = senderName },
                    to = new[] { new { email = recipientEmail, name = recipientName } },
                    subject = subject,
                    htmlContent = htmlContent
                };

                var json = JsonConvert.SerializeObject(emailData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}/smtp/email", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    Console.WriteLine($"Response: {responseContent}");
                   
                }

                var log_Email = new log_email
                {
                    SenderEmail = senderEmail,
                    senderName = senderName,
                    recipientEmail = recipientEmail,
                    recipientName = recipientName,
                    Subject = subject,
                    htmlContent = htmlContent

                };
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> SendEmailWithAttachmentAsync(string recipientEmail, string recipientName, string subject, string htmlContent,
            string attachmentBase64, string attachmentName)
        {
            try
            {
                var senderEmail = _options2.DefaultSenderEmail;
                var senderName = _options2.DefaultSenderName;

                var emailData = new
                {
                    sender = new { email = senderEmail, name = senderName },
                    to = new[] { new { email = recipientEmail, name = recipientName } },
                    subject = subject,
                    htmlContent = htmlContent,
                    attachment = new[]
                    {
                        new { content = attachmentBase64, name = attachmentName }
                    }
                };

                var json = JsonConvert.SerializeObject(emailData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}/smtp/email", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> GetAccountInfoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/account");
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        

        public List<log_email> GetEmailLogs()
        {
            return _dbPostgreContext.log_Emails.ToList();   
        }

        public void AddEmailLogs(log_email logEmail)
        {
            _dbPostgreContext.log_Emails.Add(logEmail);
            _dbPostgreContext.SaveChanges();
            return;
        }
    }
}