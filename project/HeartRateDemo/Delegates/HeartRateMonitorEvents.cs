using HeartRateDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateDemo.Delegates
{
    public class HeartRateMonitorEvents
    {
        public delegate void HeartRateMonitorConnected(object sender, HeartRateSensor sensor);
        public delegate void HeartRateMonitorDisconnected(object sender, HeartRateSensor sensor);
        public delegate void HeartRateMonitorHeartRateUpdated(object sender, HeartRateSensor sensor, uint heartRate);
    }
}
