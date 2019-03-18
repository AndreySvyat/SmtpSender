using System.Net;
using System.Net.Mail;

namespace SmtpSender
{
    //TODO add logger or openspan logger

    public class SmtpComponent
    { 
        private SmtpClient smtp { get; set; }
        //TODO use autentification tocken of encripted credentials
        private NetworkCredential creds { get; set; }


        public MailMessage mail { get; set; }
        public string username { get; set; }
        public string password { get; set; } //TODO use secure string for password
        public string host { get; set; }
        public int port { get; set; }
        public bool enableSsl { get; set; }


        public SmtpComponent() { }

        public void send()
        {
            initSmtp();
            smtp.Send(mail);
        }

        public void send(string from, string to, string subject, string body) {
            initSmtp();
            smtp.Send(from, to, subject, body);
        }

        private void initSmtp() {
            smtp = new SmtpClient(host: host, port: port);
            creds = new NetworkCredential(password: password, userName: username);
            smtp.Credentials = creds;
            smtp.EnableSsl = enableSsl;
        }

        
    }
}
