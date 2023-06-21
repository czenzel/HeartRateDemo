using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeartRateDemo.Models;
using HeartRateDemo.Delegates;

namespace HeartRateDemo.Interfaces
{
    public interface IHeartRateMonitorClient
    {
        event HeartRateMonitorEvents.HeartRateMonitorConnected Connected;
        event HeartRateMonitorEvents.HeartRateMonitorDisconnected Disconnected;
        event HeartRateMonitorEvents.HeartRateMonitorHeartRateUpdated HeartRateUpdated;

        HeartRateSensor Sensor { get; set; }
        Task Connect();
        void Disconnect();
    }
}
