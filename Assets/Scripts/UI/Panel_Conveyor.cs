using System;
using System.Diagnostics;
using TMPro;
using UnityEngine.UI;
using WI;

namespace Edukit
{
    public class Panel_Conveyor : EdukitDashboard,ISingle
    {
        public Slider Slider;
        public Action<float> onConveyorSpeedChange;
        private void Start()
        {
            Slider.onValueChanged.AddListener(ConveyorSpeedControll);
        }

        protected override void OnVariableChange(string variableName, string value)
        {
        }

        private void ConveyorSpeedControll(float value)
        {
            onConveyorSpeedChange?.Invoke(value);
        }
    }
}