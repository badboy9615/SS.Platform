using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServiceTestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //访问webservice的代理类       
            OAService.UserInfoServiceSoapSoapClient client = new OAService.UserInfoServiceSoapSoapClient();
           Console.WriteLine(client.Add(3, 3));
           Console.ReadKey();
        }
    }
}
