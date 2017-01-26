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
        public int actionNumber;

        public PlayerAction(string actionName, string value, string stage, int actionNumber)
        {
            this.actionName = actionName;
            this.stage = stage;
            this.actionNumber = actionNumber;

            double parsedValue;
            if(double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedValue))
                this.value = parsedValue;
        }

    }
}
