using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class SessionMetadata
    {
        [DataMember]
        public List<string> ChannelNames { get; set; }

        public SessionMetadata(params string[] channelNames)
        {
            ChannelNames = channelNames.ToList();
        }
    }
}
