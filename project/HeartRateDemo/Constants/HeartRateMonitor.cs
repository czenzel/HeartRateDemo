using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateDemo.Constants
{
    public static class HeartRateMonitor
    {
        public static Guid HEART_RATE_SERVICE_UUID = 0x180D.UuidFromPartial();
        public static Guid HEART_RATE_MEASURE_UUID = 0x2A37.UuidFromPartial();
    }
}
