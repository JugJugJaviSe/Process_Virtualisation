using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public class ClientFileManager : IDisposable
    {
        private StreamReader _streamReader;
        private StreamWriter _invalidWriter;

        public ClientFileManager(string csvFileName, string invalidLogsPath)
        {
            _streamReader = new StreamReader(csvFileName);
            _streamReader.ReadLine();   //skip the header
            _invalidWriter = new StreamWriter(invalidLogsPath, append: true);
        }

        public SensorSample GetNextSample()
        {
            string csvLine;

            if ((csvLine = _streamReader.ReadLine()) != null)
            {
                string[] csvLineParts = csvLine.Split(',');

                if (
                (DateTime.TryParse(csvLineParts[0], out DateTime dateTime)) &&
                (double.TryParse(csvLineParts[1], out double volume)) &&
                (double.TryParse(csvLineParts[4], out double pressure)) &&
                (double.TryParse(csvLineParts[8], out double co)) &&
                (double.TryParse(csvLineParts[9], out double no2))
                )
                {
                    SensorSample sample = new SensorSample();
                    sample.DateTime = dateTime;
                    sample.Volume = volume;
                    sample.Pressure = pressure;
                    sample.CO = co;
                    sample.NO2 = no2;
                    return sample;
                }
                else
                {
                    _invalidWriter.WriteLine(csvLine);
                    return new SensorSample();
                }
            }
            else
            {
                return null;
            }
        }

        public void WriteUnreadSensorSamples()
        {
            string csvLine;

            while ((csvLine = _streamReader.ReadLine()) != null)
            {
                _invalidWriter.WriteLine(csvLine);
            }
            _invalidWriter.Flush();
        }

        public void Dispose()
        {
            _streamReader?.Dispose();
            _invalidWriter?.Dispose();
        }
    }
}
