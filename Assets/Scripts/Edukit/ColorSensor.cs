using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Edukit
{
    public class ColorSensor : SubMachine
    {
        public EdukitChip sensingChip;
        public string Sen1PowerState;
        public string ColorSensorSensing;
        public event Action<bool> onColorSensorSensing;

        internal event Action<EdukitChip> outSensingChip;
        internal event Action<EdukitChip> onSensingChip;        

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<EdukitChip>(out var c))
                return;           
            sensingChip = c;
            //Debug.Log($"In Chip ColorSensor");
            onSensingChip?.Invoke(c);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<EdukitChip>(out var c))
                return;

            if (c == sensingChip)
            {
                sensingChip = null;
                outSensingChip?.Invoke(c);
            }
            else
            {
                Debug.Log($"Over Color Sensing!");
            }
        }

        protected override void VariableUpdateEvent(string vn, string vv)
        {
            switch (vn)
            {
                case nameof(ColorSensorSensing):
                    if (isTrue(vv))
                    {
                        //Debug.Log($"Color Sensor Sensing : {colorCount}");
                        onColorSensorSensing?.Invoke(isTrue(vv));
                    }
                    break;
            }
        }

    }
}