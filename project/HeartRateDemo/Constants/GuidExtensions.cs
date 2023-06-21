using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateDemo.Constants
{
    public static class GuidExtensions
    {
        public static Guid UuidFromPartial(this Int32 @partial)
        {
            //this sometimes returns only the significant bits, e.g.
            //180d or whatever. so we need to add the full string
            string id = @partial.ToString("X").PadRight(4, '0');

            if (id.Length == 4)
            {
                id = "0000" + id + "-0000-1000-8000-00805f9b34fb";
            }

            return Guid.ParseExact(id, "d");
        }
    }
}
