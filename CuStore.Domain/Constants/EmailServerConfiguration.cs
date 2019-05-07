using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.Domain.Constants
{
    public class EmailServerConfiguration
    {
        public string MailFromAddress = "order@custore.com";
        public bool UseSsl = true;
        public string Username = "username";
        public string Password = "password";
        public string ServerName = "smtp.custore.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = Path.GetTempPath() + @"\CuStoreSmtp";
    }
}
