using Common.Exceptions;
using Common.Models;
using Common.Services;
using Service.EventArguments;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SensorService : ISensorService
    {
        private List<string> _currentSessionChannels = new List<string>();
        private FileWriter _sensorSampleWriter;
        private FileWriter _logWriter;

        public delegate void MyEventHandler(object sender, CustomEventArgs e);

        public event MyEventHandler OnTransferStarted;
        public event MyEventHandler OnSampleReceived;
        public event MyEventHandler OnTransferCompleted;
        public event MyEventHandler OnWarningRaised;

        
        private double _pressureThreshold = double.Parse(ConfigurationManager.AppSettings["P_threshold"]);
        private double _no2Threshold = double.Parse(ConfigurationManager.AppSettings["N02_threshold"]);
        private double _coThreshold = double.Parse(ConfigurationManager.AppSettings["C0_threshold"]);
        // Allowed deviation from session mean (±%)
        private double _deviationThreshold = double.Parse(ConfigurationManager.AppSettings["DeviationThreshold"]);

        public OperationResult StartSession(SessionMetadata meta)
        {
            if (OnTransferStarted != null)
                OnTransferStarted(this, new CustomEventArgs("Session started."));

            _currentSessionChannels = meta.ChannelNames;
            _sensorSampleWriter = new FileWriter(ConfigurationManager.AppSettings["measurementsSessionCsv"], false);
            _logWriter = new FileWriter(ConfigurationManager.AppSettings["rejectsCsv"], false);

            _sensorSampleWriter.WriteLog(string.Join(",", _currentSessionChannels));
            _logWriter.WriteLog(string.Join(",", _currentSessionChannels));

            return new OperationResult(ResponseCode.ACK, SessionStatus.IN_PROGRESS);
        }

        public OperationResult PushSample(SensorSample sample)
        {
            if (OnSampleReceived != null)
                OnSampleReceived(this, new CustomEventArgs($"Sample received.({sample})"));
            try
            {
                ValidateSample(sample);
                _sensorSampleWriter.WriteSensorSample(sample);
            }
            catch (Exception ex)
            {
                _logWriter.WriteSensorSample(sample);

                if (OnWarningRaised != null)
                {
                    OnWarningRaised(this, new CustomEventArgs($"Warning raised.({ex})"));
                }
                throw ex;   //throwing so client can print it
            }
            return new OperationResult(ResponseCode.ACK, SessionStatus.IN_PROGRESS);
        }

        public OperationResult EndSession()
        {
            if (OnTransferCompleted != null)
                OnTransferCompleted(this, new CustomEventArgs("Session ended."));
            //Dispose pattern test
            /*try
            {
                using (FileStream fs = new FileStream(ConfigurationManager.AppSettings["measurementsSessionCsv"], FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("File is locked because FileWriter still has it open.\n" + ex);
            }*/

            _sensorSampleWriter.Dispose();
            _logWriter.Dispose();

            //Dispose pattern test
            /*using (var fs = new FileStream(ConfigurationManager.AppSettings["measurementsSessionCsv"], FileMode.Open, FileAccess.Write, FileShare.None))
            {
                Console.WriteLine("Success: after Dispose(), the file can be opened again.");
            }*/

            return new OperationResult(ResponseCode.ACK, SessionStatus.COMPLETED);

        }

        private void ValidateSample(SensorSample sample)
        {

            if (sample == null)
                throw new FaultException<DataFormatFault>(
                    new DataFormatFault("Sample cannot be null"),
                    "Data format error");

            if (sample.Pressure <= 0)
                throw new FaultException<ValidationFault>(
                    new ValidationFault($"Pressure must be greater than 0. Received: {sample.Pressure}"),
                    "Validation error");

            if (sample.CO < 0)
                throw new FaultException<ValidationFault>(
                    new ValidationFault($"CO must be non-negative. Received: {sample.CO}"),
                    "Validation error");

            if (sample.NO2 < 0)
                throw new FaultException<ValidationFault>(
                    new ValidationFault($"NO2 must be non-negative. Received: {sample.NO2}"),
                    "Validation error");

            if (sample.Volume <= 0)
                throw new FaultException<ValidationFault>(
                    new ValidationFault($"Volume must be greater than 0. Received: {sample.Volume}"),
                    "Validation error");
        }
    }
}
