using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security;
using SmtpComponent;
using OpenSpan.Controls;

namespace Test
{
    class Program
    {

        static (string, string)[] test { get; set; }

        static void Main(string[] args)
        {
            SmtpSSL mail = new SmtpSSL();
            mail.enableSsl = true;
            mail.port = 587;
            mail.host = "smtp.gmail.com";
            mail.username = "testcsharpsmtp@gmail.com";
            string pwdstr = "passwordsmtp";
            unsafe
            {
                fixed (char* pwd = pwdstr)
                {
                    mail.password= new SecureString(pwd,pwdstr.Length);
                }
            }

            sendfullemail(mail);
            
            Console.ReadKey();
        }

        static void sendfullemail(SmtpSSL mail)
        {
            mail.addBody(@"C:\testTemp\formail.htm");
            mail.addTo("andrey.svyatogorov@gmail.com");
            mail.addCC("andreysvyatoy@gmail.com");
            mail.addBcc("andrey.svyatogorov@masterdata.ru");
            mail.addAttachments(@"C:\testTemp\test.html", @"C:\testTemp");
            mail.displayName="andrey";
            mail.send();
        }

    }
}
