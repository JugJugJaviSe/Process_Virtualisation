using Common.Models;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SensorService : ISensorService
    {
        public OperationResult EndSession()
        {
            throw new NotImplementedException();
        }

        public OperationResult PushSample(SessionMetadata csvLine)
        {
            throw new NotImplementedException();
        }

        public OperationResult StartSession(SessionMetadata meta)
        {
            throw new NotImplementedException();
        }
    }
}
