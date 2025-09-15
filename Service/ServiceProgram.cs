using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceProgram
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(SensorService));
            serviceHost.Open();
            Console.WriteLine("Service is active");
            Console.ReadKey();

            serviceHost.Close();
            Console.WriteLine("Service is closed");
        }
    }
}
