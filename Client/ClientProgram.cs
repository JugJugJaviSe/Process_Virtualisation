using Client.Helpers;
using Common.Models;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientProgram
    {
        static void Main(string[] args)
        {
            ChannelFactory<ISensorService> channelFactory = new ChannelFactory<ISensorService>("SensorService");
            ISensorService proxy = channelFactory.CreateChannel();

            using (ClientFileManager fileManager = new ClientFileManager(ConfigurationManager.AppSettings["SensorSamplesCsv"], ConfigurationManager.AppSettings["InvalidLogs"]))
            {
                for (int i = 0; i < 100; ++i)
                {
                    SensorSample sample = fileManager.GetNextSample();
                    if (sample == null)
                    {
                        Console.WriteLine("No more data for reading from CSV.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine(sample);
                    }

                }
            }
        }
    }
}
