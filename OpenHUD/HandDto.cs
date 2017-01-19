using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud
{
    class HandDto
    {
        private String tableName { get; set; }
        private String tableType { get; set; }

        private float smallBlindValue { get; set; }
        private float bigBlindValue { get; set; }

        private double handNumber { get; set; }
        DateTime dateTime { get; set; }
        TimeZone timeZone { get; set; }

    }
}
