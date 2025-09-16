using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    public class FileWriter : IDisposable
    {
        private StreamWriter _writer;
        private bool _disposed = false;

        public FileWriter(string fileName, bool append = true)
        {
            _writer = new StreamWriter(fileName, append);
        }

        public void WriteSensorSample(SensorSample sample)
        {
            if (!_disposed)
            {
                _writer.WriteLine(sample);
            }
        }

        public void WriteLog(string message)
        {
            if (!_disposed)
                _writer?.WriteLine(message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && _writer != null)
                {
                    _writer.Flush();
                    _writer.Dispose();
                    _writer = null;
                }
                _disposed = true;
            }
        }
    }
}
