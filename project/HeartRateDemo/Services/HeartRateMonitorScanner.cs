using HeartRateDemo.Constants;
using HeartRateDemo.Models;
using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateDemo.Services
{
    public class HeartRateMonitorScanner
    {
        private readonly IBleManager _bleManager;
        private readonly ObservableList<HeartRateSensor> _sensors = new ObservableList<HeartRateSensor>();

        public HeartRateMonitorScanner(IBleManager bleManager)
        {
            _bleManager = bleManager;
        }

        public ObservableList<HeartRateSensor> Sensors => _sensors;

        public void Start()
        {
            if (_bleManager.IsScanning)
                _bleManager.StopScan();

            _sensors.Clear();

            _bleManager.Scan()
            .Subscribe(_result =>
            {
                try
                {
                    if (_result != null && _result.AdvertisementData != null &&
                        _result.AdvertisementData.ServiceUuids != null &&
                        _result.AdvertisementData.ServiceUuids.Any(a => Guid.Parse(a).Equals(HeartRateMonitor.HEART_RATE_SERVICE_UUID)))
                    {
                        _sensors.Add(new HeartRateSensor(_result));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Uncaught Exception in Bluetooth Scanner: {ex.Message}");
                }
            });
        }

        public void Stop()
        {
            if (_bleManager.IsScanning)
                _bleManager.StopScan();
        }
    }
}
