using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WI;

namespace Edukit
{
    public class Panel_M1 : EdukitDashboard, ISingle
    {
        public TextMeshProUGUI No1PowerState;
        public TextMeshProUGUI No1DelayTime;
        public TextMeshProUGUI No1Push;
        public TextMeshProUGUI No1Count;
        public TextMeshProUGUI No1ChipFull;

        protected override void OnVariableChange(string variableName, string value)
        {
            switch(variableName)
            {
                case nameof(No1PowerState):
                    MainValue.Instance.M1State = value; 
                    Debug.LogError(value);
                    No1PowerState.SetText(value);
                    break;

                case nameof(No1DelayTime):
                    MainValue.Instance.No1DelayTime = value;
                    No1DelayTime.SetText(value);
                    break;

                case nameof(No1Push):
                    MainValue.Instance.No1Push = value;
                    No1Push.SetText(value);
                    break;

                case nameof(No1Count):
                    MainValue.Instance.No1Count = value;
                    No1Count.SetText(value);
                    break;

                case nameof(No1ChipFull):
                    MainValue.Instance.No1ChipFull = value;
                    No1ChipFull.SetText(value);
                    break;
            }
        }
    }
}