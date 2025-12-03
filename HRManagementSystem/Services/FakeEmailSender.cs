using Microsoft.AspNetCore.Identity.UI.Services;

namespace HRManagementSystem.Services
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("======================================");
            Console.WriteLine("========   FAKE EMAIL SENDER   ========");
            Console.WriteLine("======================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"To: {email}");
            Console.WriteLine($"Subject: {subject}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--------------- BODY -----------------");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(htmlMessage);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("======================================");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}

