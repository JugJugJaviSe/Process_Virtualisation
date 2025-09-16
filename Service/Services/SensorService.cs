using Common.Models;
using Common.Services;
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
    public class SensorService : ISensorService
    {
        private static List<string> _currentSessionChannels = new List<string>();
        private FileWriter _sensorSampleWriter;
        private FileWriter _logWriter;

        public OperationResult StartSession(SessionMetadata meta)
        {

            _currentSessionChannels = meta.ChannelNames;
            _sensorSampleWriter = new FileWriter(ConfigurationManager.AppSettings["measurementsSessionCsv"], false);
            _logWriter = new FileWriter(ConfigurationManager.AppSettings["rejectsCsv"], false);

            _sensorSampleWriter.WriteLog(string.Join(",", _currentSessionChannels));
            _logWriter.WriteLog(string.Join(",", _currentSessionChannels));

            return new OperationResult(ResponseCode.ACK, SessionStatus.IN_PROGRESS);
        }

        public OperationResult PushSample(SensorSample sample)
        {
            try
            {
                ValidateSample(sample);
                _sensorSampleWriter.WriteSensorSample(sample);
            }
            catch (Exception ex)
            {
                _logWriter.WriteSensorSample(sample);
                Console.WriteLine(ex.ToString());
            }
            return new OperationResult(ResponseCode.ACK, SessionStatus.IN_PROGRESS);
        }

        public OperationResult EndSession()
        {
            _sensorSampleWriter.Dispose();
            _logWriter.Dispose();

            return new OperationResult(ResponseCode.ACK, SessionStatus.COMPLETED);
        }

        private void ValidateSample(SensorSample sample)
        {

            if (sample == null)
                throw new Exception("Sample cannot be null");

            if (sample.Pressure <= 0)
                throw new Exception("Pressure must be greater than 0");

            if (sample.CO < 0)
                throw new FaultException("CO must be non-negative");

            if (sample.NO2 < 0)
                throw new FaultException("NO2 must be non-negative");

            if (sample.Volume <= 0)
                throw new Exception("Volume must be greater than 0");
        }
    }
}
