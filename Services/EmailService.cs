
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NoteTakingApp.OptionsModels;
using System.Net.Mail;
using System.Net;

namespace NoteTakingApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetEmailLink, string toEmail)
        {
            string htmlTemplate = $@"
    <!DOCTYPE html>
    <html lang='tr'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Şifre Sıfırlama</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f9;
                color: #333;
                margin: 0;
                padding: 0;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background-color: #fff;
                border-radius: 10px;
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                overflow: hidden;
            }}
            .email-header {{
                background-color: #007BFF; /* Mavi renk */
                color: #fff;
                text-align: center;
                padding: 20px;
            }}
            .email-header h1 {{
                margin: 0;
                font-size: 24px;
            }}
            .email-body {{
                padding: 20px;
            }}
            .email-body p {{
                line-height: 1.6;
                font-size: 16px;
                margin: 10px 0;
            }}
            .reset-button {{
                display: inline-block;
                padding: 12px 20px;
                background-color: #007BFF; /* Mavi renk */
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
                margin: 20px auto;
                font-weight: bold;
            }}
            .reset-button:hover {{
                background-color: #0056b3; /* Daha koyu mavi renk */
            }}
            .email-footer {{
                text-align: center;
                padding: 10px;
                background-color: #f4f4f9;
                font-size: 12px;
                color: #666;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='email-header'>
                <h1>Şifre Sıfırlama</h1>
            </div>
            <div class='email-body'>
                <p>Merhaba,</p>
                <p>Notlar uygulamanız için bir şifre sıfırlama talebi aldık. Şifrenizi sıfırlamak için aşağıdaki butona tıklayın:</p>
                <p style='text-align: center;'>
                    <a href='{resetEmailLink}' class='reset-button'>Şifremi Sıfırla</a>
                </p>
                <p>Eğer bu talebi siz yapmadıysanız, lütfen bu e-postayı görmezden gelin.</p>
                <p>Teşekkürler,<br><strong>Notes App Ekibi</strong></p>
            </div>
            <div class='email-footer'>
                <p>Bu e-posta otomatik olarak oluşturulmuştur, lütfen yanıtlamayın.</p>
            </div>
        </div>
    </body>
    </html>";

           
        }

    }
}
