using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace Factory
{
    public class Chip : MonoBehaviour
    {
        public enum State
        {
            Unknown,
            Red,
            White,
            Yellow,
            Orange,
        }
        public State currentState;
        
        private Rigidbody rb;
        private Collider col;
        private MeshRenderer mr;
        public Material mat_Red;
        public Material mat_White;
        public Material mat_Unknown;
        public Material mat_Yellow;
        public Material mat_Orange;
        public Transform dicePos;
        internal bool isSensing;

        public GameObject[] FruitObj;
      
        public GameObject coverCap;
        public float speed = 1.0f; // 이동 속도
        
        private int countNumber;
        private int m1Number;
        private int m2Number;
        private int m3Number;

        private ProcessingState processingState; // 가공 상태
        private QualityState qualityState;      // 품질 상태
        private InsideState insideState;        // 내용물 상태

        //private static int sucessCount;//양품갯수

        public void SetMachineNumber(int machineNumber, int mNum)
        {
            switch (mNum)
            {
                case 1: m1Number = machineNumber; break;
                case 2: m2Number = machineNumber; break;
                case 3: m3Number = machineNumber; break;
                case 0: countNumber = machineNumber; break;
            }
        }

        private void Start()
        {
            this.SetProcessingState(ProcessingState.Processing);
            this.SetQualityState(QualityState.NotApplicable);

            
            mat_Red = Resources.Load<Material>("Materials/Chip_Red");
            mat_White= Resources.Load<Material>("Materials/Chip_White");
            mat_Unknown = Resources.Load<Material>("Materials/Chip_White");
            mat_Yellow = Resources.Load<Material>("Materials/Chip_Yellow");
            mat_Orange = Resources.Load<Material>("Materials/Chip_Orange");


            //하위오브젝트 찾아서 넣기
            List<GameObject> fruitList = new List<GameObject>();

            // 현재 스크립트가 있는 오브젝트의 하위 오브젝트들을 찾아서 리스트에 추가
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                fruitList.Add(child.gameObject);
            }
            // 리스트를 배열로 변환하여 FruitObj에 할당
            FruitObj = fruitList.ToArray();


           
        }

        public enum ProcessingState
        {
            Processing, // 가공 중: 반출기부터 가공기까지
            Inspecting, // 검사 중: 비전센서, 비파괴검사 ~~ 이후까지
            Drop, // 검사 중: 비전센서, 비파괴검사 ~~ 이후까지
            Completed   // 완료: 적재되었다.
        }

        public enum QualityState
        {
            NotApplicable, // 아직 가공 중인 경우
            ProductSucess, // 아직 가공 중인 경우
            Good,          // 양품
            Defective      // 불량
        }
        
        public enum InsideState
        {
            None, Apple, Banana, Orange,
        }

        public int GetMachineNumber(int mNum)
        {
            switch (mNum)
            {
                case 1: return m1Number;
                case 2: return m2Number;
                case 3: return m3Number;
                case 0: return countNumber;
                default: return -1; // Invalid mNum case
            }
        }
        
        private void Awake()
        {
            mr = GetComponent<MeshRenderer>();
            isSensing = false;
            rb = GetComponent<Rigidbody>();

            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            
            col = GetComponent<Collider>();
            if (col == null)
            {
                col = gameObject.AddComponent<BoxCollider>(); // BoxCollider 추가
            }
            
            ////내용물 보일지 안 보일지 제어
            //for (int i = 0; i < 4; i++)
            //{
            //    Transform child = FruitObj[1].GetChild(i);
            //    child.gameObject.SetActive(false); 
            //}
            
            //캔 뚜껑 보일지 안 보일지 제어
            coverCap.transform.parent = transform;
            coverCap.SetActive(false);
        }
        
        public void coverCapState(bool state)
        {
            if (state)
            {
                coverCap.SetActive(true);
            }
        }

        public void appleState(InsideState type, float appletime)
        {
            StartCoroutine(appleStateCoroutine(type, appletime));
            SetInsideState(type);
        }
        
        IEnumerator appleStateCoroutine(InsideState type, float time)
        {
            float interval = time / 4;
            switch (type)
            {
                case InsideState.None:
                    {
                        SetState(State.White);

                        FruitObj[0].SetActive(true);
                        yield return null;
                        //for (int i = 0; i < 4; i++)
                        //{
                        //    Transform child = FruitObj[0].GetChild(i);
                        //    child.gameObject.SetActive(true); // 하위 오브젝트 활성화
                        //    yield return new WaitForSeconds(interval); // 다음 오브젝트 활성화 전까지 대기
                        //}
                        break;
                    }
                case InsideState.Apple:
                    {
                        SetState(State.Red);
                        FruitObj[1].SetActive(true);
                        yield return null;
                        //for (int i = 0; i < 4; i++)
                        //{
                        //    Transform child = FruitObj[1].GetChild(i);
                        //    child.gameObject.SetActive(true); // 하위 오브젝트 활성화
                        //    yield return new WaitForSeconds(interval); // 다음 오브젝트 활성화 전까지 대기
                        //}
                        break;
                    }
                case InsideState.Banana:
                    {
                        SetState(State.Yellow);
                        FruitObj[2].SetActive(true);
                        yield return null;
                        //for (int i = 0; i < 4; i++)
                        //{
                        //    Transform child = FruitObj[2].GetChild(i);
                        //    child.gameObject.SetActive(true); // 하위 오브젝트 활성화
                        //    yield return new WaitForSeconds(interval); // 다음 오브젝트 활성화 전까지 대기
                        //}
                        break;
                    }
                case InsideState.Orange:
                    {
                        SetState(State.Orange);
                        FruitObj[3].SetActive(true);
                        yield return null;
                        //for (int i = 0; i < 4; i++)
                        //{
                        //    Transform child = FruitObj[3].GetChild(i);
                        //    child.gameObject.SetActive(true); // 하위 오브젝트 활성화
                        //    yield return new WaitForSeconds(interval); // 다음 오브젝트 활성화 전까지 대기
                        //}
                        break;
                    }
            }
            
            
        }

        public void SetState(State s)
        {
            switch (s)
            {
                case State.Unknown:
                    mr.material = mat_Unknown;
                    break;
                case State.Red:
                    mr.material = mat_Red;
                    break;
                case State.White:
                    mr.material = mat_White;
                    break;
                case State.Yellow:
                    mr.material = mat_Yellow;
                    break;
                case State.Orange:
                    mr.material = mat_Orange;
                    break;
            }
            currentState = s;
        }


        public bool onHand;
        public void OnGrab(Transform grabPoint)
        {
            onHand = true;
            transform.position = grabPoint.position;
            transform.SetParent(grabPoint);
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("OnGrab");
        }
        
        internal void HandOff()
        {
            onHand = false;
            transform.SetParent(null);
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("HandOff");
        }
        
        // 칩이 특정 상태가 될 때 호출될 메서드
        void OnTriggerEnter(Collider other)
        {
            // 콜라이더가 BOTTOM 태그를 가진 경우
            if (other.CompareTag("PRODUCT"))
            {
                this.SetProcessingState(ProcessingState.Inspecting);
                // BOTTOM 태그가 감지되었을 때 수행할 동작
                HandleBottomTagDetected(other);
            }

            if (other.CompareTag("SUCESS"))
            {
                this.SetProcessingState(ProcessingState.Completed);
                this.SetQualityState(QualityState.Good);                
                gameObject.SetActive(false);
                MainValue.Instance.sucessCount++;
            }

            // 콜라이더가 DROP 태그를 가진 경우
            if (other.CompareTag("DROP"))
            {
                Debug.Log("colliderDrop"+this.GetMachineNumber(0));
                this.SetQualityState(QualityState.Defective);
                this.SetProcessingState(ProcessingState.Drop);
                gameObject.SetActive(false);
            }

            // 콜라이더가 DROP 태그를 가진 경우
            //if (other.CompareTag("AISucess"))
            //{                
            //    //this.SetQualityState(QualityState.Defective);
            //    this.SetProcessingState(ProcessingState.Completed);
            //    if(this.GetQualityState() == QualityState.Good)
            //    {
            //        MainValue.Instance.AIsucessCount++;
            //    }
            //    gameObject.SetActive(false);
                
            //}
        }
        
        void HandleBottomTagDetected(Collider collider)
        {
            this.SetProcessingState(Chip.ProcessingState.Inspecting);
            
            if(this.GetMachineNumber(1)==0 || this.GetMachineNumber(2)==0 || this.GetMachineNumber(3)==0 )
                this.SetQualityState(Chip.QualityState.Defective);
            else
            {
                
                this.SetQualityState(Chip.QualityState.ProductSucess);
            }
                


            //this.SetProcessingState(ProcessingState.Completed);
            Debug.Log("chip info :"  + "    " + this.GetMachineNumber(0)
                      + "    " + this.GetMachineNumber(1)
                      + "    " + this.GetMachineNumber(2)
                      + "    " + this.GetMachineNumber(3)
                      + "    " + this.GetQualityState()
                      + "    " + this.GetProcessingState());
        }

        public void SetProcessingState(ProcessingState newState)
        {
            processingState = newState;
        }
        
        public ProcessingState GetProcessingState()
        {
            return processingState;
        }
        
        public void SetQualityState(QualityState newState)
        {
            qualityState = newState;
        }
        
        public QualityState GetQualityState()
        {
            return qualityState;
        }
        
        public void SetInsideState(InsideState newState)
        {
            insideState = newState;
        }
        
        public InsideState GetInsideState()
        {
            return insideState;
        }

        
    }
}
