using System;
using System.IO;
using System.Net;
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
            Console.WriteLine(Path.Combine(@"C:\testTemp","name"+".zip"));
            Console.WriteLine(Path.GetDirectoryName(@"C:\testTemp\installdocker.sh"));
            
            Console.ReadKey();
        }

    }
}
