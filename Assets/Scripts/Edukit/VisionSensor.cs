using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edukit
{
    public class VisionSensor : SubMachine
    {
        public string Sen2PowerState;
        public string DiceValue;
        public string DiceComparisonValue;
        public event Action<string> onChangeDiceValue;

        protected override void VariableChangeEvent(string variableName, string variableValue)
        {
            switch (variableName)
            {
                case nameof(DiceValue):
                    //Debug.Log($"DiceValue: {variableValue}");
                    onChangeDiceValue?.Invoke(variableValue);
                    break;
            }
        }
    }
}