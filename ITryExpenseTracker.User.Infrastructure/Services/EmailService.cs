using ITryExpenseTracker.User.Abstractions;
using ITryExpenseTracker.User.Abstractions.Services;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.User.Infrastructure.Services;
public class EmailService : IEmailService {

    readonly EmailConfiguration _emailConfig;

    public EmailService(IOptions<EmailConfiguration> emailConfigOptions) {
        _emailConfig = emailConfigOptions.Value;
    }

    #region SendNewPassword    
    public async Task SendNewPassword(string toMail, string userName, string password) {

        var model = $"Hi {userName}<br />this is your new password {password}";
        await SendEmail(_emailConfig.MailFrom, new List<string> { toMail }, null, "New password", model, null);
    }
    #endregion

    #region SendEmail
    public async Task<string?> SendEmail(string from, List<string> tos, List<string> bccs, string subject, string message, string[] attachments = null) {

        var mMessage = new MimeMessage();
        mMessage.From.Add(new MailboxAddress(from, from));


        foreach (var to in tos.Where(w => w.Length > 0)) {
            mMessage.To.Add(new MailboxAddress(to, to));
        }

        if (bccs != null && bccs.Count > 0) {
            foreach (var bcc in bccs.Where(w => w.Length > 0)) {
                mMessage.Bcc.Add(new MailboxAddress(bcc, bcc));
            }
        }

        var bb = new BodyBuilder();
        bb.HtmlBody = message;

        mMessage.Subject = subject;
        mMessage.Body = bb.ToMessageBody();
        

        if (!string.IsNullOrEmpty(_emailConfig.SmtpUser)
            && !string.IsNullOrEmpty(_emailConfig.SmtpPassword)
            ) 
        {
            using (var client = new SmtpClient()) {
                
                client.Connect(host: _emailConfig.SmtpServer, 
                                port: _emailConfig.SmtpPort,
                                options: MailKit.Security.SecureSocketOptions.Auto);

                client.Authenticate(_emailConfig.SmtpUser, _emailConfig.SmtpPassword);

                client.Send(mMessage);
                client.Disconnect(true);
            }
        }
        else {
            using (var client = new SmtpClient()) {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.SmtpPort, false);

                client.Send(mMessage);
                client.Disconnect(true);
            }
        }

        return null;
    }
    #endregion

    #region TestEmail
    public async Task TestEmail() {
        await SendEmail(_emailConfig.MailFrom, new List<string> { _emailConfig.MailFrom }, null, "test", "test", null);
    }
    #endregion
}

