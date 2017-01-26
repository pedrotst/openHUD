using System;
using System.Collections.Generic;
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
        public string playerName;

        public PlayerAction(string actionName, string value, string stage, string playerName)
        {
            this.actionName = actionName;
            this.stage = stage;
            this.playerName = playerName;

            double parsedValue;
            if(double.TryParse(value, out parsedValue))
                this.value = parsedValue;
        }

    }
}
