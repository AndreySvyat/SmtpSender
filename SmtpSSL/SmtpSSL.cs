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
            }
        }
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string displayName { get; set; }
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public string password
        {
            get
            {
                return creds.Password;
            }
            set
            {
                creds.Password = value;
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
            mail.From = new MailAddress(username, displayName);
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
        public void addBody(bool isHtmlBody, string body)
        {
            mail.IsBodyHtml = isHtmlBody;
            mail.Body = body;
        }
        /// <summary>
        /// Defines the message body based on an external file located at the specified path.
        /// </summary>
        /// <param name="path">defines file location</param>
        [MemberVisibilityAttribute(MemberVisibilityLevel.DefaultOff)]
        public void addBody(string path)
        {
            string body = File.ReadAllText(path);
            mail.Body = body;
            mail.IsBodyHtml = true;
        }
        /// <summary>
        /// Attach file or archived directory with specified path.
        /// </summary>
        /// <exception cref="FileLoadException">
        /// will be thrown if specified file is system file or device
        /// </exception>
        /// <param name="path"></param>
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
            mail.Attachments.Add(new Attachment(path));
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
    }
}

