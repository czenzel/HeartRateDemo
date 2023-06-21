using HeartRateDemo.Models;
using HeartRateDemo.Delegates;
using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeartRateDemo.Constants;
using HeartRateDemo.Extensions;
using HeartRateDemo.Interfaces;

namespace HeartRateDemo.Services
{
    public class HeartRateMonitorClient : IHeartRateMonitorClient
    {
        private readonly IBleManager _bleManager;

        public HeartRateMonitorClient(IBleManager bleManager)
        {
            _bleManager = bleManager;
        }

        HeartRateSensor _sensor = null;
        public HeartRateSensor Sensor
        {
            get
            {
                return _sensor;
            }
            set
            {
                _sensor = value;
            }
        }

        public event HeartRateMonitorEvents.HeartRateMonitorConnected Connected;
        public event HeartRateMonitorEvents.HeartRateMonitorDisconnected Disconnected;
        public event HeartRateMonitorEvents.HeartRateMonitorHeartRateUpdated HeartRateUpdated;

        private void Listen()
        {
            if (Sensor == null ||
                Sensor.Peripheral == null)
                return;

            Sensor.Peripheral.WhenConnected().Subscribe(_device =>
            {
                Connected?.Invoke(this, Sensor);
            });

            Sensor.Peripheral.WhenDisconnected().Subscribe(_device =>
            {
                Disconnected?.Invoke(this, Sensor);
            });
        }

        public async Task Connect()
        {
            if (Sensor == null ||
                Sensor.Peripheral == null)
                return;

            Listen();

            await Sensor.Peripheral.ConnectAsync(new ConnectionConfig(false));
            await Task.Delay(TimeSpan.FromMilliseconds(2));
            await Subscribe();
        }

        public void Disconnect()
        {
            if (Sensor == null ||
                Sensor.Peripheral == null)
                return;

            if (Sensor.Peripheral.Status != ConnectionState.Disconnected)
            {
                Unsubscribe();
                Sensor.Peripheral.CancelConnection();
            }
        }

        private IDisposable _heartRateDisposable;

        private async Task Subscribe()
        {
            if (Sensor == null ||
                Sensor.Peripheral == null)
                return;

            if (Sensor.Peripheral.Status != ConnectionState.Connected)
                return;

            BleCharacteristicInfo heartRateCharacteristic = await Sensor.Peripheral.GetCharacteristicAsync(HeartRateMonitor.HEART_RATE_SERVICE_UUID.ToString(),
                HeartRateMonitor.HEART_RATE_MEASURE_UUID.ToString());

            _heartRateDisposable = Sensor.Peripheral.NotifyCharacteristic(heartRateCharacteristic, true)
                .Subscribe(_result =>
                {
                    try
                    {
                        byte[] data = _result.Data;

                        if (data != null && data.Length > 0)
                        {
                            uint heartRateMeasurement = data.DecodeHeartRate();
                            HeartRateUpdated?.Invoke(this, Sensor, heartRateMeasurement);
                            System.Diagnostics.Debug.WriteLine($"Heart Rate Monitor > Heart Rate Updated: {heartRateMeasurement} bpm");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Add internal exception handling here
                    }
                });
        }

        private void Unsubscribe()
        {
            _heartRateDisposable?.Dispose();
            _heartRateDisposable = null;
        }
    }
}
