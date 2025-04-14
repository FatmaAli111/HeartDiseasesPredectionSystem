using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.EmailSender
{
   public class EmailSettings
   {

            public string FromEmail { get; set; }
            public string FromName { get; set; }
            public string Password { get; set; }
            public string ToEmail { get; set; }
            public string SmtpHost { get; set; }
            public int SmtpPort { get; set; }
        
    
    
    
    }

    
}
