using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeartRateDemo.Models;

namespace HeartRateDemo.Interfaces
{
    public interface IHeartRateMonitorScanner
    {
        ObservableList<HeartRateSensor> Sensors { get; }
        void Start();
        void Stop();
    }
}
