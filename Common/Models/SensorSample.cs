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

        [DataMember] public DateTime? DateTime { get; set; }
        [DataMember] public double? Volume { get; set; }
        [DataMember] public double? LightLevel { get; set; }
        [DataMember] public double? TemperatureDHT { get; set; }
        [DataMember] public double? Pressure { get; set; }
        [DataMember] public double? TemperatureBMP { get; set; }
        [DataMember] public double? RelativeHumidity { get; set; }
        [DataMember] public double? AirQuality { get; set; }
        [DataMember] public double? CO { get; set; }
        [DataMember] public double? NO2 { get; set; }

        public override string ToString()
        {
            return $"{Volume}, {Pressure}, {CO}, {NO2}, {DateTime}";
        }
    }
}
