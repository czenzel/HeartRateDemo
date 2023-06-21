using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiny.BluetoothLE;

namespace HeartRateDemo.Models
{
    public class HeartRateSensor
    {
        public HeartRateSensor(IPeripheral peripheral)
        {
            Peripheral = peripheral;
        }

        public HeartRateSensor(ScanResult result)
        {
            ScanResult = result;
            Peripheral = result.Peripheral;
        }

        public IPeripheral Peripheral { get; private set; }

        public ScanResult ScanResult { get; private set; }

        public string Name => Peripheral != null ?
            Peripheral.Name :
            string.Empty;

        public string Uuid => Peripheral != null ?
            Peripheral.Uuid :
            string.Empty;

        public int Rssi => ScanResult != null ?
            ScanResult.Rssi :
            0;
    }
}
