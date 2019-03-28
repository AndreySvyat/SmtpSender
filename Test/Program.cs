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
            SmtpClient cl = new SmtpClient("smtp.gmail.com", 587);
            cl.EnableSsl = true;
            cl.Credentials = new NetworkCredential("testcsharpsmtp@gmail.com", "passwordsmtp");
            cl.Send("testcsharpsmtp@gmail.com", "andrey.svyatogorov@masterdata.ru", "kdfjghalksdjf", "askjdfhlkasjdfj");

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
            mail.send("andreysvyatoy@gmail.com", "andrey.svyatogorov@gmail.com", "test", "some text for test message!");

            sendfullemail(mail);
            
            Console.ReadKey();
        }

        static void sendfullemail(SmtpSSL mail)
        {
            mail.addBody(false, "new mail test");
            mail.addTo("andrey.svyatogorov@gmail.com");
            mail.addCC("andreysvyatoy@gmail.com");
            mail.addBcc("andrey.svyatogorov@masterdata.ru");
            mail.addAttachments(@"C:\testTemp\test.html", @"C:\testTemp");
            mail.displayName="andrey";
            mail.send();
        }

    }
}
