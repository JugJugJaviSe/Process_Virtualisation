using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class SensorSample
    {
        public SensorSample() { }

        [DataMember] public DateTime DateTime { get; set; }
        [DataMember] public double Volume { get; set; }
        [DataMember] public double Pressure { get; set; }
        [DataMember] public double CO { get; set; }
        [DataMember] public double NO2 { get; set; }

        public override string ToString()
        {
            return $"{Volume}, {CO}, {NO2}, {Pressure}, {DateTime}";
        }
    }
}
