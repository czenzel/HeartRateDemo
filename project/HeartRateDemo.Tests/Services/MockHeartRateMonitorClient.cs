using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeartRateDemo.Delegates;
using HeartRateDemo.Interfaces;
using HeartRateDemo.Models;

namespace HeartRateDemo.Tests.Services
{
    public class MockHeartRateMonitorClient : IHeartRateMonitorClient
    {
        public HeartRateSensor Sensor { get; set; } = null;

        public event HeartRateMonitorEvents.HeartRateMonitorConnected Connected;
        public event HeartRateMonitorEvents.HeartRateMonitorDisconnected Disconnected;
        public event HeartRateMonitorEvents.HeartRateMonitorHeartRateUpdated HeartRateUpdated;

        public async Task Connect()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            Assert.Pass();
        }

        public void Disconnect()
        {
            Assert.Pass();
        }
    }
}
