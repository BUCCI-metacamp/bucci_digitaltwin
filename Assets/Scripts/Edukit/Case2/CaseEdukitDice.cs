using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class CaseEdukitDice : MonoBehaviour
    {
        public int topValue;
        public Material mat_Unknown;
        public Material mat_Known;

        public Vector3[] valueToRot = new Vector3[6];
        public void SetValue(int v)
        {
            if (v <= 0)
            {
                //GetComponent<MeshRenderer>().material = mat_Unknown;
                return;
            }
            Debug.Log($"Set Dice Value : {v}");
            //isSensing = true;
            topValue = v;
            GetComponent<MeshRenderer>().material = mat_Known;
            var rot = valueToRot[v - 1];
            var quar = Quaternion.Euler(rot);
            transform.localRotation = quar;
        }
    }
}