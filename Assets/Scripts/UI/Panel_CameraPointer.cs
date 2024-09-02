using System;
using UnityEngine;
using UnityEngine.UI;

namespace Edukit
{
    public class Panel_CameraPointer : MonoBehaviour
    {
        public event Action<SubMachine> onClickM1;
        public event Action<SubMachine> onClickM2;
        public event Action<SubMachine> onClickM3;
        public event Action<SubMachine> onClickColorSensor;
        public event Action<SubMachine> onClickVisionSensor;
        public event Action<SubMachine> onClickDashboard;
        public event Action<SubMachine> onClickConveyor;
        //public event Action<SubMachine> onClickHMI;

        public Edukit edukit;
        
        private void Awake()
        {
            edukit = FindObjectOfType<Edukit>();
            Button M1 = transform.Find(nameof(M1)).GetComponent<Button>();
            Button M2 = transform.Find(nameof(M2)).GetComponent<Button>();
            Button M3 = transform.Find(nameof(M3)).GetComponent<Button>();
            Button Dashboard = transform.Find(nameof(Dashboard)).GetComponent<Button>();
            Button ColorSensor = transform.Find(nameof(ColorSensor)).GetComponent<Button>();
            Button VisionSensor = transform.Find(nameof(VisionSensor)).GetComponent<Button>();
            Button Conveyor = transform.Find(nameof(Conveyor)).GetComponent<Button>();
            //Button HMI = transform.Find(nameof(Panel_HMI)).GetComponent<Button>();

            M1.onClick.AddListener(OnClickM1);
            M2.onClick.AddListener(OnClickM2);
            M3.onClick.AddListener(OnClickM3);
            ColorSensor.onClick.AddListener(OnClickColorSensor);
            VisionSensor.onClick.AddListener(OnClickVisionSensor);
            Dashboard.onClick.AddListener(OnClickDashboard);
            Conveyor.onClick.AddListener(OnClickConveyor);
           // HMI.onClick.AddListener(OnClickConveyor);
        }

        void OnClickM1()
        {
            onClickM1?.Invoke(edukit.m1);
        }

        void OnClickM2()
        {
            onClickM2?.Invoke(edukit.m2);
        }

        void OnClickM3()
        {
            onClickM3?.Invoke(edukit.m3);
        }

        void OnClickColorSensor()
        {
            onClickColorSensor?.Invoke(edukit.colorSensor);
        }

        void OnClickVisionSensor()
        {
            onClickVisionSensor?.Invoke(edukit.visionSensor);
        }

        void OnClickDashboard()
        {
            onClickDashboard?.Invoke(edukit.etc);
        }

        void OnClickConveyor()
        {
            onClickConveyor?.Invoke(edukit.conv);
        }

        void OnClickHMI()
        {
            //onClickHMI?.Invoke(edukit.conv);
        }
    }
}