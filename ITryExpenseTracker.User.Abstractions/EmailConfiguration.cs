using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.User.Abstractions;
public class EmailConfiguration {

    public string SmtpServer { get; set; }

    public string SmtpUser { get; set; }

    public string SmtpPassword { get; set; }

    public int SmtpPort { get; set; }
    public string MailFrom { get; set; }
}

