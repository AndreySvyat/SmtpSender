using System.Net.Mail;
using System.ComponentModel;

namespace SmtpComponent
{
    public partial class MailMessage : Component
    {
        public MailMessage()
        {
            InitializeComponent();
        }
        public MailMessage(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public System.Net.Mail.MailMessage mailMessage {  get; set; }
    }
}
