using TMPro;
using WI;
using UnityEngine;


namespace Edukit
{
    public class Panel_ETC : EdukitDashboard, ISingle
    {
        public TextMeshProUGUI StartState;
        public TextMeshProUGUI ResetState;
        public TextMeshProUGUI EmergencyState;
        public TextMeshProUGUI InputLimit;
        public TextMeshProUGUI DataTime;
        static int abc = 0;


        protected override void OnVariableChange(string variableName, string value)
        {
            switch(variableName)
            {
                case nameof(StartState):
                    MainValue.Instance.StartState = value;
                    StartState.SetText(value);
                    break;

                case nameof(ResetState):
                    MainValue.Instance.ResetState = value;
                    ResetState.SetText(value);
                    break;

                case nameof(EmergencyState):
                    Debug.LogError(abc + "sss" + value);
                    abc++;
                    MainValue.Instance.EmergencyState = value;
                    EmergencyState.SetText(value);
                    break;

                case nameof(InputLimit):
                    MainValue.Instance.InputLimit = value;
                    InputLimit.SetText(value);
                    break;

                case nameof(DataTime):
                    MainValue.Instance.DataTime = value;
                    DataTime.SetText(value);
                    break;
            }
        }
    }
}