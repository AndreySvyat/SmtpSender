using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using OpenSpan.TypeManagement;

namespace SmtpComponent
{
    [System.Drawing.ToolboxBitmapAttribute(typeof(SmtpSSL), "SMTP.bmp")]
    [ToolboxItemFilterAttribute("OpenSpan.Automation.Automator", ToolboxItemFilterType.Custom)]
    public partial class SmtpSSL : Component
    {
        public SmtpSSL()
        {
            InitializeComponent();
        }
        public SmtpSSL(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private SmtpClient smtp { get; set; }
        //TODO use autentification tocken of encripted credentials
        private NetworkCredential creds { get; set; }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public MailMessage mail { get; set; }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string username { get; set; }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string password { get; set; } //TODO use secure string for password

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string host { get; set; }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public int port { get; set; }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public bool enableSsl { get; set; }




        public void send()
        {
            initSmtp();
            smtp.Send(mail);
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void send(string from, string to, string subject, string body)
        {
            initSmtp();
            smtp.Send(from, to, subject, body);
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        private void initSmtp()
        {
            smtp = new SmtpClient(host: host, port: port);
            creds = new NetworkCredential(password: password, userName: username);
            smtp.Credentials = creds;
            smtp.EnableSsl = enableSsl;
        }
    }
}
