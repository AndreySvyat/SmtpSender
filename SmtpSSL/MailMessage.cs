using System.ComponentModel;
using System.Net.Mail;
using OpenSpan.TypeManagement;

namespace SmtpComponent
{

    [ToolboxItemFilterAttribute("OpenSpan.Automation.Automator", ToolboxItemFilterType.Custom)]
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

        private System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string subject
        {
            get
            {
                return mailMessage.Subject;
            }
            set
            {
                mailMessage.Subject = value;
            }
        }
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string body
        {
            get
            {
                return mailMessage.Body;
            }
            set
            {
                mailMessage.Body = value;
            }
        }
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public bool isBodyHtml
        {
            get
            {
                return mailMessage.IsBodyHtml;
            }
            set
            {
                mailMessage.IsBodyHtml = value;
            }
        }
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOn)]
        public MailAddress from
        {
            get
            {
                if (mailMessage.From == null)
                {
                    mailMessage.From = new MailAddress("example@mail.ru"); 
                }
                return mailMessage.From;
            }
            set
            {
                mailMessage.From = value;
            }
        }
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public MailAddressCollection to
        {
            get
            {
                return mailMessage.To;
            }

            set
            {
                foreach(MailAddress mail in value)
                {
                    mailMessage.To.Add(mail);
                }
            }
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public MailAddress GetAddress(string address, string name)
        {
            if (name == null) return new MailAddress(address);
            return new MailAddress(address, name);
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addToAddresses(params MailAddress[] mailAddresses) {
            foreach (MailAddress mailAddress in mailAddresses) {
                mailMessage.To.Add(mailAddress);
            }
        }
    }
}
