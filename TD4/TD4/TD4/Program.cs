using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TD4.ServiceReference1;

namespace TD4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalculatorSoapClient csclient = new CalculatorSoapClient();
            Console.WriteLine(csclient.Add(2, 5));
            Console.ReadLine();
        }
    }
}
