using System;
using System.Security;
using SmtpComponent;

namespace Test
{
    class Program
    {

        static (string, string)[] test { get; set; }

        static void Main(string[] args)
        { 

            SecureString sString; ;
            unsafe
            {
                fixed (char* p = Console.ReadLine())
                {
                    sString = new SecureString(p, 10);
                }
            }
            Console.ReadKey();
        }
    }
}
