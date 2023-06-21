using HeartRateDemo.Interfaces;
using HeartRateDemo.Tests.Services;

namespace HeartRateDemo.Tests
{
    public class HeartRateMonitorClientTests
    {
        private IHeartRateMonitorClient _heartRateMonitorClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _heartRateMonitorClient = new MockHeartRateMonitorClient();
        }

        [Test]
        public void HeartRateMonitorClient_Connect_IsPass_Test()
        {
            _heartRateMonitorClient.Connect();
        }

        [Test]
        public void HeartRateMonitorClient_Disconnect_IsPass_Test()
        {
            _heartRateMonitorClient.Disconnect();
        }

        [Test]
        public void HeartRateMonitorClient_Sensor_IsNull_Test()
        {
            Assert.IsNull(_heartRateMonitorClient.Sensor);
        }
    }
}