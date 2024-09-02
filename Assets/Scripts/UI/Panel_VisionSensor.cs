using TMPro;
using WI;

namespace Edukit
{
    public class Panel_VisionSensor : EdukitDashboard, ISingle
    {
        public TextMeshProUGUI Sen2PowerState;
        public TextMeshProUGUI DiceValue;
        public TextMeshProUGUI DiceComparisonValue;

        protected override void OnVariableChange(string variableName, string value)
        {
            switch(variableName)
            {
                case nameof(Sen2PowerState):
                    MainValue.Instance.Sen2PowerState = value;
                    Sen2PowerState.SetText(value);
                    break;
                    
                case nameof(DiceValue):
                    MainValue.Instance.DiceValue = value;
                    DiceValue.SetText(value);
                    break;

                case nameof(DiceComparisonValue):
                    MainValue.Instance.DiceComparisonValue = value;
                    DiceComparisonValue.SetText(value);
                    break;
            }
        }
    }
}