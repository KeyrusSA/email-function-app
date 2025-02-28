using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Email.Notifications
{
    public class EmailTimerTrigger
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public EmailTimerTrigger(ILoggerFactory loggerFactory, HttpClient httpClient)
        {
            _logger = loggerFactory.CreateLogger<EmailTimerTrigger>();
            _httpClient = httpClient;
        }

        [Function("EmailTimerTrigger")]
        public async Task Run([TimerTrigger("0 15 15 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            var endpoint = "https://web-infoflo-api-production-zan-agdcbufgfuaueegt.southafricanorth-01.azurewebsites.net/api/email/send-email-notifications";
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation($"Successfully hit the endpoint: {endpoint}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error hitting the endpoint: {endpoint}. Exception: {ex.Message}");
            }
        }
    }
}