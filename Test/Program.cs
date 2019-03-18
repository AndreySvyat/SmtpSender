using SmtpSender;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("test");
            SmtpComponent component = new SmtpComponent();
            component.enableSsl = true;
            component.host = "smtp.gmail.com";
            component.port = 587;
            component.username = "testcsharpsmtp@gmail.com";
            component.password = "csharpsendertest";
            component.enableSsl = true;
            component.send(component.username, "andrey.svyatogorov@masterdata.ru", "test mess", "Text should be here, but it is test");
            Console.WriteLine("good");
            Console.ReadKey();
        }
    }
}
