using HeartRateDemo.Interfaces;
using HeartRateDemo.Tests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateDemo.Tests
{
    public class HeartRateMonitorScannerTests
    {
        private IHeartRateMonitorScanner _heartRateMonitorScanner;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _heartRateMonitorScanner = new MockHeartRateMonitorScanner();
        }

        [Test]
        public void HeartRateMonitorScanner_Start_IsPass_Test()
        {
            _heartRateMonitorScanner.Start();
        }

        [Test]
        public void HeartRateMonitorScanner_Stop_IsPass_Test()
        {
            _heartRateMonitorScanner.Stop();
        }

        [Test]
        public void HeartRateMonitorScanner_Sensors_Length_Test()
        {
            Assert.That(_heartRateMonitorScanner.Sensors.Count, Is.EqualTo(0));
        }
    }
}
