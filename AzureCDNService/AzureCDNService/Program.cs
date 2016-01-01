using AzureCDNService.App;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCDNService
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new StartOptions();
            options.Urls.Add("http://localhost:1337");
            options.Urls.Add("http://127.0.0.1:1337");

            var SleipnirWebApi = WebApp.Start<Startup>(options);

            Console.WriteLine("Started Azure CDN Upload Server at " + options.Urls.Last());
            Console.WriteLine("Press Any Key To Exit");

            Console.Read();
        }
    }
}
