using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeartRateDemo.Interfaces;
using HeartRateDemo.Models;

namespace HeartRateDemo.Tests.Services
{
    public class MockHeartRateMonitorScanner : IHeartRateMonitorScanner
    {
        public ObservableList<HeartRateSensor> Sensors { get; set; } = new ObservableList<HeartRateSensor>();

        public void Start()
        {
            Assert.Pass();
        }

        public void Stop()
        {
            Assert.Pass();
        }
    }
}
