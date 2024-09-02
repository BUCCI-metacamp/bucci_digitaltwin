using System;
using UnityEngine;
using UnityEngine.UI;

namespace Edukit
{
    public class EdukitDashboard : MonoBehaviour
    {
        public bool lookLock;
        private void Awake()
        {
            Button close = transform.Find(nameof(close)).GetComponent<Button>();
            close.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void Update()
        {
            if(!lookLock)
            transform.LookAt(Camera.main.transform);
        }

        internal void Connect(SubMachine machine)
        {
            machine.SetDashboard(this, OnVariableChange);
        }

        protected virtual void OnVariableChange(string variableName, string value)
        {
            throw new NotImplementedException();
        }
    }
}