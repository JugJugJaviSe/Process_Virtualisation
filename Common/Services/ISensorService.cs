using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    [ServiceContract]
    public interface ISensorService
    {
        [OperationContract]
        OperationResult StartSession(SessionMetadata meta);

        [OperationContract]
        OperationResult PushSample(SensorSample sample);

        [OperationContract]
        OperationResult EndSession();
    }
}
