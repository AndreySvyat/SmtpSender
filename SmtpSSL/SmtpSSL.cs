using System.ComponentModel;
using System.Collections.Generic;
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

        //TODO use autentification tocken of encripted credentials
        private SmtpClient smtp = new SmtpClient(); 
        private NetworkCredential creds = new NetworkCredential();
        private MailMessage mail = new MailMessage();
        private List<string> tempFiles = new List<string>();

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string username
        {
            get
            {
                return creds.UserName;
            }
            set
            {
                creds.UserName = value;
                mail.From = new MailAddress(value);
            }
        }
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string displayName
        {
            get
            {
                return mail.From.DisplayName;
            }
            set
            {
                mail.From = new MailAddress(username, value);
            }
        }
        //TODO use secure string for password
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public System.Security.SecureString password
        {
            get
            {
                return creds.SecurePassword;
            }
            set
            {
                creds.SecurePassword = value;
            }
        } 
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

        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void send()
        {
            smtp.Credentials = creds;
            smtp.Send(mail);
            mail.Dispose();
            removeTempFiles();
        }
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void send(string from, string to, string subject, string body)
        {
            smtp.Credentials = creds;
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
                        attachFolderArchive(path);
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
        private string attachFolderArchive(string path)
        {
            string dirName = Path.GetFileName(path);
            string archivePath = Path.Combine(Path.GetTempPath(), dirName + ".zip");
            tempFiles.Add(archivePath);

            ZipFile.CreateFromDirectory(path, archivePath);
            
            mail.Attachments.Add(new Attachment(archivePath));
            return archivePath;
        }
        private void removeTempFiles()
        {
            foreach (string tmp in tempFiles) File.Delete(tmp);
        }

        //TODO handle all exceptions add logging
    }
}

