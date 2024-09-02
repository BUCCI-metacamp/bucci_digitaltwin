using System;
using System.Collections;
using UnityEngine;

namespace Factory
{
    public class M03 : MonoBehaviour
    {
        public event Action<float> convStop; // 컨베이어 벨트를 잠깐 정지하는 이벤트
        public event Action<int, string, float> StartMoving; //푸셔를 움직이는 이벤트
        
        private bool M3Powerstate;
        public float M3SpeedRatio;
        
        public float M03Time; //가공시간
        private float M03Duration; //푸셔작동시간
        public int M03Number;
        
        public Chip sensingChip;
        private bool flag;
        public GameObject rotationEffect; // 회전 이펙트 오브젝트

        private void Start()
        {
            flag = true;
            M03Duration = -0.05f * M3SpeedRatio + 6f;
            M3Powerstate = false;
            GameObject.Find("StartButton").GetComponent<StartButton>().onStartButton += M3Power;

            //Setting
            ControlSettingView settingView = GameObject.Find("SettingView").GetComponent<ControlSettingView>();
            settingView.CMachineTime += OnOKButtonClickedTime; // 가공시간 
            settingView.CMachineDuration += OnOKButtonClickedDuration; //공정속도

            //Setting
            AISetting AISetting = GameObject.Find("AIView").GetComponent<AISetting>();
            AISetting.CMachineTime += OnOKButtonClickedTime; //가공시간
            AISetting.CMachineDuration += OnOKButtonClickedDuration; //공정속도
        }

        void OnOKButtonClickedTime(float loadCapacity) // 가공시간 
        {
            // Parse the loadCapacity string to an integer and assign it to TotalAmount            
            M03Time = loadCapacity;
            Debug.Log("M03Time updated to: " + M03Time);
        }

        void OnOKButtonClickedDuration(float loadCapacity)//공정속도
        {
            M3SpeedRatio = loadCapacity;
            M03Duration = -0.05f * M3SpeedRatio + 6f;
            Debug.Log("M01Duration updated to: " + M03Duration);
        }

        private void M3Power(bool state)
        {
            M3Powerstate = state;
        }
        
        IEnumerator WaitPusherCoroutine(Chip sensingChip)
        {
            StartMoving?.Invoke(3, "ON", M03Duration); 
            // 내려오는 만큼 만큼 대기
            yield return new WaitForSecondsRealtime(M03Duration);
            sensingChip.coverCapState(true);
            float elapsedTime = 0f;
            // 이펙트 활성화
            rotationEffect.SetActive(true);
            Quaternion initialRotation = sensingChip.transform.rotation;
            //Debug.Log("initialRotation":+initialRotation.x+initialRotation.y+initialRotation.z);
            Quaternion finalRotation = initialRotation * Quaternion.Euler(0, 0, 180); // 회전

            while (elapsedTime < M03Time)
            {
                // elapsedTime / duration에 따라 회전 각도를 선형 보간
                sensingChip.transform.rotation = Quaternion.Lerp(initialRotation, finalRotation, elapsedTime / M03Time);
                elapsedTime += Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }

            // 정확하게 한 바퀴 회전하도록 회전값 설정
            sensingChip.transform.rotation = finalRotation;
            rotationEffect.SetActive(false);

            //yield return new WaitForSeconds(M03Time);
            StartMoving?.Invoke(3, "OFF", M03Duration);

            yield return new WaitForSeconds(M03Duration);

            flag = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Chip>(out var c) || !M3Powerstate || c.GetMachineNumber(3)!=0 || !flag) return;
            flag = false; 
            sensingChip = c;
            
            if (sensingChip == null)
            {
                Debug.LogError("Failed to assign sensingChip.");
                return;
            }
            c.SetMachineNumber(M03Number, 3);
            convStop?.Invoke(M03Time + M03Duration); 
            StartCoroutine(WaitPusherCoroutine(c)); 
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Chip>(out var c))
            {
                sensingChip = null;
            }
        }
    }
}