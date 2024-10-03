using FinancialAssistant.DataTransfer.Password;
using FinancialAssistant.DataTransfer.User;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;

namespace FinancialAssistant.EmailService.Implementation;

public class EmailService : IEmailService
{
    public async Task SendEmail(PasswordResetEmailDto passwordResetEmail, CancellationToken cancellationToken)
    {
        var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Admin", "kupregion3@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("Client", passwordResetEmail.Email));
            emailMessage.Subject = "Change password";
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = 
                    """
                    <!DOCTYPE html>
                    <html lang="ru">
                    <head>
                        <meta charset="UTF-8">
                        <meta name="viewport" content="width=device-width, initial-scale=1.0">
                        <title>Сброс пароля</title>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: white;
                                color: yellow;
                                margin: 0;
                                padding: 20px;
                            }
                            .container {
                                background-color: white;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 0 10px rgba(255, 255, 255, 0.2);
                                text-align: center;
                            }
                            h1 {
                                margin-bottom: 20px;
                            }
                            .code {
                                font-size: 24px;
                                font-weight: bold;
                                color: yellow;
                                background-color: #444;
                                padding: 10px;
                                border-radius: 4px;
                                display: inline-block;
                                margin: 20px 0;
                            }
                            p {
                                margin: 10px 0;
                            }
                            .footer {
                                margin-top: 20px;
                                font-size: 12px;
                                color: black;
                            }
                        </style>
                    </head>
                    <body>
                        <div class="container">
                            <h1>Сброс пароля</h1>
                            <p>Здравствуйте!</p>
                            <p>Вы запросили сброс пароля для вашей учетной записи.</p>
                            <p>Ваш код для сброса пароля:</p>
                    """ + "\n\r" +
                            $"""<div class="code"> {passwordResetEmail.Code} </div>""" +
                    """
                            <p>Пожалуйста, используйте этот код, чтобы сбросить ваш пароль.</p>
                            <p>Если вы не запрашивали сброс пароля, просто проигнорируйте это сообщение.</p>
                            <div class="footer">С уважением, ваша команда поддержки.</div>
                        </div>
                    </body>
                    </html>
                    """
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls, cancellationToken);
                await client.AuthenticateAsync("kupregion3@gmail.com", "jiuc ztct uyzg odza", cancellationToken);
                await client.SendAsync(emailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true, cancellationToken);
            }
    }
}