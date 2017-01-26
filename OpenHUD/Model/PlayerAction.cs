using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud.Model
{
    class PlayerAction
    {
        public string actionName;
        public double? value = null;
        public string stage;

        public PlayerAction(string actionName, string value, string stage)
        {
            this.actionName = actionName;
            this.stage = stage;

            double parsedValue;
            if(double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedValue))
                this.value = parsedValue;
        }

    }
}
