using System.ComponentModel;
using System.IO;
using System.IO.Compression;
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

        private SmtpClient smtp = new SmtpClient();
        //TODO use autentification tocken of encripted credentials
        private NetworkCredential creds { get; set; }

        private MailMessage mail { get; set; }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string username { get; set; }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string password { get; set; } //TODO use secure string for password

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string host
        {
            get
            {
                return smtp.Host;
            }
            set
            {
                smtp.Host = value;
            }
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public int port
        {
            get
            {
                return smtp.Port;
            }
            set
            {
                smtp.Port = value;
            }
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public bool enableSsl
        {
            get
            {
                return smtp.EnableSsl;
            }
            set
            {
                smtp.EnableSsl = value;
            }
        }

        private void Connect()
        {
            creds = new NetworkCredential(password: password, userName: username);
            smtp.Credentials = creds;
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void send()
        {
            Connect();
            smtp.Send(mail);
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void send(string from, string to, string subject, string body)
        {
            Connect();
            smtp.Send(from, to, subject, body);
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addTo(string address)
        {
            mail.To.Add(address);
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addTo(string name, string address)
        {
            mail.To.Add(new MailAddress(address, name));
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addCC(string address)
        {
            mail.CC.Add(address);
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addCC(string name, string address)
        {
            mail.CC.Add(new MailAddress(address, name));
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addBcc(string address)
        {
            mail.Bcc.Add(address);
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addBcc(string name, string address)
        {
            mail.Bcc.Add(new MailAddress(address, name));
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addBody(string body)
        {
            mail.Body = body;
        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addBody(string body, bool isHtmlBody)
        {
            mail.IsBodyHtml = isHtmlBody;
            addBody(body);

        }

        //TODO add body from file

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addAttachments(string path)
        {
            FileAttributes fileAttr = File.GetAttributes(path);
            switch (fileAttr) {
                case FileAttributes.ReadOnly:
                case FileAttributes.Hidden: 
                case FileAttributes.Archive:
                case FileAttributes.Temporary:
                case FileAttributes.SparseFile:
                case FileAttributes.ReparsePoint:
                case FileAttributes.Offline:
                case FileAttributes.Encrypted:
                case FileAttributes.NoScrubData:
                case FileAttributes.Normal:
                    {
                        attachFile(path);
                        break;
                    }
                case FileAttributes.Directory:
                    {
                        attachAllFilesInDir(path);
                        break;
                    }
                case FileAttributes.Device:
                    {
                        throw new FileLoadException("This id device don't try attach it to the email!");
                    }
                case FileAttributes.System:
                    {
                        throw new FileLoadException("System files can't be sent!");
                    }
            }

        }

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addAttachments(params string[] paths)
        {
            foreach (string path in paths) addAttachments(path);
        }

        private void attachFile(string path)
        {
            try
            {
                mail.Attachments.Add(new Attachment(path));
            }
            catch (FileNotFoundException e) {
                //TODO add logging
            }
        }

        private void attachAllFilesInDir(string path)
        {
            string dirName = Path.GetFileName(path);
            string archivePath = Path.Combine(path, dirName + ".zip");
            ZipArchive attachments = ZipFile.Open(archivePath,ZipArchiveMode.Create);

            fillArchive(path, attachments);

            mail.Attachments.Add(new Attachment(archivePath));
        }

        private void fillArchive(string path, ZipArchive archive)
        {
            foreach (string file in Directory.GetFiles(path))
            {
                if (File.GetAttributes(file) == FileAttributes.Directory) { fillArchive(file, archive); }
                archive.CreateEntryFromFile(file, Path.GetFileName(file));
            }
        }
    }
}

