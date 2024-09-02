using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Factory
{
    public class Case2Nondestructive : CaseSubMachine
    {
        public Chip sensingChip;
        public string Sen1PowerState;
        public string ColorSensorSensing;
        public event Action<bool> onColorSensorSensing;

        internal event Action<Chip> outSensingChip;
        internal event Action<Chip> onSensingChip;

        public TextMeshPro ContentName;

        private void OnTriggerEnter(Collider other)
        { 
            if (!other.TryGetComponent<Chip>(out var c))
                return;
            string str = string.Format("{0}", c.GetInsideState());
            ContentName.text = str;
            sensingChip = c;
            //Debug.Log($"In Chip ColorSensor");
            onSensingChip?.Invoke(c);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Chip>(out var c))
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

        

    }
}