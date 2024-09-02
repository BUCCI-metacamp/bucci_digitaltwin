
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Edukit
{
    public class SubMachine : MonoBehaviour
    {
        public Transform CameraPoint;
        internal bool alreadyActiveDashboard => dashboard.gameObject.activeSelf;
        public EdukitDashboard dashboard;
        public event Action<string, string> onVariableChanged;
        public event Action<string, string> onVariableUpdate;
        public event Action<SubMachine> onClickUITrigger;
        protected const string TRUE = "true";
        protected void Awake()
        {
            CameraPoint = transform.Find(nameof(CameraPoint));
            onVariableChanged += VariableChangeEvent;
            onVariableUpdate += VariableUpdateEvent;
        }

        protected virtual void VariableUpdateEvent(string vn, string vv)
        {

        }
        protected virtual void VariableChangeEvent(string variableName, string variableValue)
        {
        }

        public bool isTrue(string value)
        {
            return value == TRUE;
        }

        HashSet<string> initVariables = new HashSet<string>();
        public void SetValue(string variableName, string value)
        {
            var targetField = this.GetType().GetField(variableName);
            var prevValue = targetField.GetValue(this);
            if (!initVariables.Contains(variableName))
            {
                initVariables.Add(variableName);
                onVariableChanged?.Invoke(variableName, value);
            }
            else
            {
                if (prevValue.ToString() != value)
                {
                    onVariableChanged?.Invoke(variableName, value);
                }
            }
            onVariableUpdate?.Invoke(variableName, value);
            targetField.SetValue(this, value);
        }

        internal void ActiveDashboard()
        {
            dashboard.gameObject.SetActive(true);
        }

        internal void SetDashboard(EdukitDashboard edukitDashboard, Action<string, string> onVariableChange)
        {
            dashboard = edukitDashboard;
            initVariables.Clear();
            this.onVariableChanged += onVariableChange;
        }
    }
}