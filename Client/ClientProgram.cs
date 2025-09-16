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

            SessionMetadata sessionMetadata = new SessionMetadata("Volume", "CO", "NO2", "Pressure", "DateTime");

            OperationResult operationResult = proxy.StartSession(sessionMetadata);//EX ovde?

            if (operationResult.ResponseCode == ResponseCode.ACK && operationResult.SessionStatus == SessionStatus.IN_PROGRESS)
            {
                Console.WriteLine("Session with server started successfully!");
            }

            using (ClientFileManager fileManager = new ClientFileManager(ConfigurationManager.AppSettings["SensorSamplesCsv"], ConfigurationManager.AppSettings["InvalidLogs"]))
            {
                fileManager.WriteUnreadSensorSamplesHeader(string.Join(",", sessionMetadata.ChannelNames));

                for (int i = 0; i < 100; ++i)
                {
                    SensorSample sample = fileManager.GetNextSample();
                    if (sample == null)
                    {
                        Console.WriteLine("No more data for reading from CSV.");
                        break;
                    }//EOF

                    if (sample.DateTime != default)
                    {
                        OperationResult result2 = proxy.PushSample(sample);
                        if (result2.ResponseCode == ResponseCode.NACK)
                        {
                            Console.WriteLine("Server did not acnowledge sample successfully!");
                        }
                        else
                        {
                            //Console.WriteLine("Server acknowledged sample successfully!");
                        }

                    }
                }

                fileManager.WriteUnreadSensorSamples();
            }

            OperationResult result3 = proxy.EndSession();

            if (result3.ResponseCode == ResponseCode.ACK && result3.SessionStatus == SessionStatus.COMPLETED)
            {
                Console.WriteLine("Session ended successfully!");
            }
            else
            {
                Console.WriteLine("Session ended unsuccessfully!");
            }
        }
    }
}
