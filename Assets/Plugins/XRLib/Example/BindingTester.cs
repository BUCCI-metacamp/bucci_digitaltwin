using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using WI;

namespace WI.Test
{
    public class BindingTester : MonoBehaviour
    {
        public int value1;
        public float value2;
        public string value3;
        public Vector3 value4;

        private void Awake()
        {
            FieldBinder.Regist(this).Binding(nameof(value1), ChangeEvent1);
            FieldBinder.Regist(this).Binding(nameof(value2), ChangeEvent2);
            FieldBinder.Regist(this).Binding(nameof(value3), ChangeEvent3);
            FieldBinder.Regist(this).Binding(nameof(value4), ChangeEvent4);
        }

        void ChangeEvent4()
        {
            Debug.Log("Vector3 field Changed");
        }
        void ChangeEvent3()
        {
            Debug.Log("string field Change");
        }

        void ChangeEvent1()
        {
            Debug.Log("Int Field Change");
        }

        void ChangeEvent2()
        {
            Debug.Log("float field Change");
        }
    }
}
