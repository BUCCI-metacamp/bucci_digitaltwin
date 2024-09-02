using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edukit
{
    public class M7 : SubMachine
    {
        public string GreenLampState;
        public string YellowLampState;
        public string RedLampState;
        public GameObject light_Green;
        public GameObject light_Yellow;
        public GameObject light_Red;

        protected override void VariableChangeEvent(string variableName, string variableValue)
        {
            switch (variableName)
            {
                case nameof(GreenLampState):
                    MainValue.Instance.GreenLampState = variableValue;
                    light_Green.gameObject.SetActive(isTrue(variableValue));
                    break;

                case nameof(YellowLampState):
                    MainValue.Instance.YellowLampState = variableValue;
                    light_Yellow.gameObject.SetActive(isTrue(variableValue));
                    break;

                case nameof(RedLampState):
                    MainValue.Instance.RedLampState = variableValue;
                    light_Red.gameObject.SetActive(isTrue(variableValue));
                    break;
            }
        }
    }
}