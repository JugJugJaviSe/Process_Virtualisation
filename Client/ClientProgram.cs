using Client.Helpers;
using Common.Exceptions;
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

            OperationResult operationResult = proxy.StartSession(sessionMetadata);

            if (operationResult.ResponseCode == ResponseCode.ACK && operationResult.SessionStatus == SessionStatus.IN_PROGRESS)
            {
                Console.WriteLine("Session with server started successfully!");
            }

            try
            {
                using (ClientFileManager fileManager = new ClientFileManager(ConfigurationManager.AppSettings["SensorSamplesCsv"], ConfigurationManager.AppSettings["InvalidLogs"]))
                {
                    //Dispose pattern test
                    //throw new Exception("Simulated connection drop!");
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
                            try
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
                            catch (FaultException<DataFormatFault> ex)
                            {
                                Console.WriteLine(ex.Detail.Message);
                            }
                            catch (FaultException<ValidationFault> ex)
                            {
                                Console.WriteLine(ex.Detail.Message);
                            }
                            catch (FaultException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            //Console.WriteLine(sample);
                        }
                        else if (sample.DateTime == default)
                        {
                            //Console.WriteLine("INVALID " + sample);//invalid sample
                        }
                    }

                    fileManager.WriteUnreadSensorSamples();
                }
            }
            catch (Exception ex)
            {
                //Dispose pattern test
                Console.WriteLine("Caught expected exception: " + ex.Message);
            }

            //Dispose pattern test
            /*using (var fs = new FileStream(ConfigurationManager.AppSettings["InvalidLogs"], FileMode.Open, FileAccess.Write, FileShare.None))
            {
                Console.WriteLine("Success: file was closed after exception.");
            }*/

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
