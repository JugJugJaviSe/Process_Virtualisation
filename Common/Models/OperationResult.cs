using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public enum ResponseCode
    {
        [EnumMember]
        ACK,
        [EnumMember]
        NACK
    }

    [DataContract]
    public enum SessionStatus
    {
        [EnumMember]
        IN_PROGRESS,
        [EnumMember]
        COMPLETED
    }

    [DataContract]
    public class OperationResult
    {
        public OperationResult(ResponseCode responseCode, SessionStatus sessionStatus)
        {
            ResponseCode = responseCode;
            SessionStatus = sessionStatus;
        }

        [DataMember]
        public ResponseCode ResponseCode { get; set; }

        [DataMember]
        public SessionStatus SessionStatus { get; set; }
    }
}
