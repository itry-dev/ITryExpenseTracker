using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.User.Abstractions.Services;
public interface IEmailService {

    Task<string?> SendEmail(string from, List<string> tos, List<string> bccs, string subject, string message, string[] attachments = null);

    Task SendNewPassword(string toMail, string userName,string password);

    Task TestEmail();
}

