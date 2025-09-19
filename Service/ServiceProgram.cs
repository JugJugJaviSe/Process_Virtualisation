using Service.Helpers;
using Service.Services;
using System;
using System.Configuration;
using System.ServiceModel;
namespace Service
{
    public class ServiceProgram
    {
        static void Main(string[] args)
        {
            SensorService sensorService = new SensorService();
            FileWriter _logWriter = new FileWriter(ConfigurationManager.AppSettings["logFile"],false);

            sensorService.OnTransferStarted += (s, e) =>
            {
                _logWriter.WriteLog($"{DateTime.Now} ->  {e.Message}");
                Console.WriteLine($"{DateTime.Now} -> {e.Message}");
            };

            sensorService.OnSampleReceived += (s, e) =>
            {
                _logWriter.WriteLog($"{DateTime.Now} ->  {e.Message}");
                Console.WriteLine($"{DateTime.Now} -> {e.Message}");
            };

            sensorService.OnWarningRaised += (s, e) =>
            {
                _logWriter.WriteLog($"{DateTime.Now} ->  {e.Message}");
                Console.WriteLine($"{DateTime.Now} -> {e.Message}");
            };

            sensorService.OnTransferCompleted += (s, e) =>
            {
                _logWriter.WriteLog($"{DateTime.Now} -> {e.Message}");
                Console.WriteLine($"{DateTime.Now} -> {e.Message}");
            };

            sensorService.PressureSpike += (s, e) =>
            {
                _logWriter.WriteLog($"{DateTime.Now} -> {e.Message}");
                Console.WriteLine($"{DateTime.Now} -> {e.Message}");
            };
            sensorService.OutOfBandWarning += (s, e) =>
            {
                _logWriter.WriteLog($"{DateTime.Now} -> {e.Message}");
                Console.WriteLine($"{DateTime.Now} -> {e.Message}");
            };

            ServiceHost serviceHost = new ServiceHost(sensorService);
            serviceHost.Open();
            Console.WriteLine("Service is active");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            // Dispose the log writer
            _logWriter.Dispose();

            serviceHost.Close();
            Console.WriteLine("Service is closed");
        }
    }
}
