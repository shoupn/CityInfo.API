using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.MailService
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = "admin@admin.com";
        private string _mailFrom = "no_reply@company.com";

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail From {_mailFrom} to {_mailTo}, using Cloud Mail Service");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
