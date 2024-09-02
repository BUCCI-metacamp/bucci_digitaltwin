using TMPro;
using WI;

namespace Edukit
{
    public class Panel_M3 : EdukitDashboard, ISingle
    {
        public TextMeshProUGUI No3ChipArrival;
        public TextMeshProUGUI No3PowerState;
        public TextMeshProUGUI No3Count;
        public TextMeshProUGUI No3Motor1Position;
        public TextMeshProUGUI No3Motor2Position;
        public TextMeshProUGUI No3Gripper;
        public TextMeshProUGUI No3Motor1Action;
        public TextMeshProUGUI No3Motor2Action;

        protected override void OnVariableChange(string variableName, string value)
        {
            switch(variableName)
            {
                case nameof(No3ChipArrival):
                    MainValue.Instance.No3ChipArrival = value;
                    No3ChipArrival.SetText(value);
                    break;
                case nameof(No3PowerState):
                    MainValue.Instance.M3State = value;
                    No3PowerState.SetText(value);
                    break;
                case nameof(No3Count):
                    MainValue.Instance.No3Count = value;
                    No3Count.SetText(value);
                    break;
                case nameof(No3Motor1Position):
                    MainValue.Instance.No3Motor1Position = value;
                    No3Motor1Position.SetText(value);
                    break;
                case nameof(No3Motor2Position):
                    MainValue.Instance.No3Motor2Position = value;
                    No3Motor2Position.SetText(value);
                    break;
                case nameof(No3Gripper):
                    MainValue.Instance.No3Gripper = value;
                    No3Gripper.SetText(value);
                    break;
                case nameof(No3Motor1Action):
                    MainValue.Instance.No3Motor1Action = value;
                    No3Motor1Action.SetText(value);
                    break;
                case nameof(No3Motor2Action):
                    MainValue.Instance.No3Motor2Action = value;
                    No3Motor2Action.SetText(value);
                    break;
            }
        }
    }
}