using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using UnityEditor;
using System;
using TMPro;
using UnityEngine.UI;

namespace Factory
{
    public class ManageCase2 : BaseMono<ManageCase2>
    {
        public M01 M1F1;
        public M01 M1F2;
        public M01 M1F3; 
        public Chip lastChip;

        public int TotalAmount;
        // private Chip[] runningChips;
        // Chip 타입의 리스트 선언
        List<Chip> runningChips = new List<Chip>();
        private int nextIndex = 0; // 배열에 추가될 다음 인덱스
        private readonly object lockObject = new object(); // 동기화를 위한 객체

        public event Action stopCreateChip;
        public event Action<int> LedChange;


        private bool chipFull;

        private float startTime;
        private float operationTime;


        //Table
        public TableCreator tableCreator;
        public TextMeshProUGUI[] tableResult;

        public event Action<List<AIReceiveData>> AiVIewer;
        public AIReceiveView AIVewer;
        public GameObject AIButon;
        public GameObject HeserBar;
        public Button aiButton1;

        public TMP_InputField cost;
        public TMP_InputField price;


        TabManager tab;

        public TextMeshProUGUI StartTimetext;

        public TextMeshProUGUI uiText;
        void Start()
        {
            chipFull = false;
            M1F1.onCreateChip += AddChipToArray;
            M1F2.onCreateChip += AddChipToArray;
            M1F3.onCreateChip += AddChipToArray;
            //runningChips = new Chip[TotalAmount];

            ControlSettingView settingView = GameObject.Find("SettingView").GetComponent<ControlSettingView>();
            settingView.TotalOutput += SettingClickedCount; //생산총갯수


            var startButton = GameObject.Find("StartButton")?.GetComponent<StartButton>();
            startButton.onStartButton += RecordStartTime; //저는 버튼 눌렀을 때부터 시간 기록해서 이거 추가했어요

            SecenChange RestConnect = GameObject.Find("SecenChange").GetComponent<SecenChange>();
            RestConnect.ResetStart += RestStart;

            tab = GameObject.Find("AIViewTab").GetComponent<TabManager>();

            var topic = "simulation/optimal/data";
            FindObjectOfType<CaseMqtt>().SubscriptionTopic(topic, ReceiveData);

            ConnectMQTT();
        }

        void ConnectMQTT()
        {
            var defalutHost = "158.247.241.162";
            var defalutPort = "1883";
            var defalutTopics = "simulation/optimal/data";

            var mqtt = FindObjectOfType<CaseMqtt>();
            Debug.Log($" Connect to {defalutHost}:{defalutPort} {defalutTopics}");
            mqtt.Connect(defalutHost, defalutPort, defalutTopics);
            //FindObjectOfType<Panel_Option>().SetDefaultValues(defalutHost, defalutPort, defalutTopics[0]);
        }

        private void Update()
        {
            if (!chipFull) return;


            if (MainValue.Instance.GetSucessCount() < TotalAmount)
            {
                Debug.LogError("아직 컴플리트"+ MainValue.Instance.GetSucessCount() +"완료 못함"+ TotalAmount);
                return;
            }

                

            LedChange?.Invoke(1);
            Time.timeScale = 0; // 게임의 모든 동작을 정지            
            CloseFactory();
            //InvestigateChips(); 

            Debug.LogError("No more space in the array to add new chips.");
        }

        void RestStart(bool reset)
        {
            if(reset == false)
            {
                chipFull = false;
                nextIndex = 0;
                runningChips = new List<Chip>();
            }
        }

        public int GetIndex()
        {
            return nextIndex;
        }
        
        public bool GetBool()
        {
            return chipFull;
        }

        private void RecordStartTime(bool state)
        {
            if (!state) return;
            startTime = Time.time;
            // 현재 시간 가져오기
            DateTime currentTime = DateTime.Now;
            // 현재 시간 출력
            //Debug.Log("현재 시간: " + currentTime.ToString("yyyy-MM-dd HH:mm:ss"));
            string str = string.Format(currentTime.ToString("yyyy-MM-dd HH:mm:ss"));
            StartTimetext.text = str;
        }

        private void CloseFactory()
        {
            if (!chipFull) return;
            operationTime = Time.time - startTime;
            Time.timeScale = 0; // 게임의 모든 동작을 정지
            InvestigateChips(operationTime);
            CreateChipTable();
            Debug.LogError("완료");
            chipFull = false;  
        }


        void SettingClickedCount(string loadCapacity) //생산총갯수
        {
            // Parse the loadCapacity string to an integer and assign it to TotalAmount            
            TotalAmount = int.Parse(loadCapacity);
            //runningChips = new Chip[TotalAmount];
            Debug.Log("M01Time updated with load capacity: " + TotalAmount);  
        }

        void DisplayText(string message, Vector2 position)
        {
            uiText.text = message;
            RectTransform rectTransform = uiText.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position; // 텍스트의 위치 설정
        }

        void AddChipToArray(Chip chip)
        {
            lock (lockObject)
            {
                //string str = string.Format("Total{0}::::Sucess{1}", TotalAmount, MainValue.Instance.GetSucessCount());
                //DisplayText(str, new Vector2(100, 100));
                if (MainValue.Instance.sucessCount == 10)   
                {
                    Debug.LogError("TotalAmount:::::::::::::::::" + TotalAmount);
                }
                if (MainValue.Instance.GetSucessCount() >= TotalAmount) //더이상 칩을 만들지 않는다.
                {
                    chipFull = true;
                    stopCreateChip?.Invoke();
                    Time.timeScale = 0; // 게임의 모든 동작을 정지
                    //Modal.Instance.ShowModal("완료111111");
                    return;
                }

                //if (nextIndex == TotalAmount)
                //{
                //    Modal.Instance.ShowModal("완료22222");
                //    return;
                //}
                   
                int currentIndex = nextIndex;
                runningChips.Add(chip);
                //Debug.LogError("runningChips.Count"+ runningChips.Count);
                //runningChips[currentIndex] = chip;
                if(chip.GetMachineNumber(0)==0) chip.SetMachineNumber(currentIndex+1, 0);
                nextIndex++;
                chip.SetProcessingState(Chip.ProcessingState.Processing);
                chip.SetQualityState(Chip.QualityState.NotApplicable);
            }
        }
        
        void InvestigateChips(float Time)
        {
            int goodCount = 0;
            int defectiveCount = 0;
            int notApplicableCount = 0;
            int productSucessCount = 0;
            
            foreach (Chip chip in runningChips)
            {
                if (chip != null)
                {
                    int chipNumber = chip.GetMachineNumber(0);
                    int m1DefectiveCount = chip.GetMachineNumber(1);
                    int m2DefectiveCount = chip.GetMachineNumber(2);
                    int m3DefectiveCount = chip.GetMachineNumber(3);
                    Chip.QualityState quality = chip.GetQualityState();
                    Chip.ProcessingState process = chip.GetProcessingState();

                    Debug.Log($"Chip Number: {chipNumber}, Quality: {quality}, Process: {process}");

                    if (quality == Chip.QualityState.Good)
                    {
                        goodCount++;
                    }
                    else if (quality == Chip.QualityState.Defective)
                    {
                        defectiveCount++;
                    }
                    else if(quality == Chip.QualityState.ProductSucess)
                    {
                        productSucessCount++;
                    }
                    else if (quality == Chip.QualityState.NotApplicable)
                    {
                        notApplicableCount++;
                    }
                    
                    if (m1DefectiveCount == 0)
                    {
                        m1DefectiveCount++;
                    }
                    else if (m2DefectiveCount == 0)
                    {
                        m2DefectiveCount++;
                    }
                    else if (m3DefectiveCount == 0)
                    {
                        m3DefectiveCount++;
                    }

                    if (chipNumber == runningChips.Count)
                    {
                        Debug.Log("Total Good Chips: "+goodCount
                                                      + ",    Total Defective Chips: "+defectiveCount
                                                      +",     Total NotApplicable Chips: "+notApplicableCount);
            
                        Debug.Log("Total M1DefectiveCount: "+m1DefectiveCount
                                                            + ",    Total M2DefectiveCount: "+m2DefectiveCount
                                                            +",     Total M3DefectiveCount: "+m3DefectiveCount);

                        string str = string.Format("Total Good Chips: \n" + goodCount
                                                      + ",    Total Defective Chips: \n" + defectiveCount
                                                      + ",     Total NotApplicable Chips: \n" + notApplicableCount
                                                      + "Total M1DefectiveCount: \n" + m1DefectiveCount
                                                        + ",    Total M2DefectiveCount: \n" + m2DefectiveCount
                                                         + ",     Total M3DefectiveCount: " + m3DefectiveCount);
                        //Modal.Instance.ShowModal(str);

                        CreateChipTable();
                        // 현재 시간 출력                     
                        TimeSpan timeSpan = TimeSpan.FromSeconds(operationTime);
                        int hours = timeSpan.Hours;
                        int minutes = timeSpan.Minutes;
                        int seconds = timeSpan.Seconds;
                        string tablestr = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds); // 결과창 시간 표출
                        tableResult[0].text = tablestr;

                        tablestr = string.Format("{0}", goodCount); // 결과창 양품 수 표출
                        tableResult[1].text = tablestr;

                        tablestr = string.Format("{0}", runningChips.Count); // 투입수
                        tableResult[2].text = tablestr;

                        float goodRatio = ((float)goodCount * 100.0f) / (float)runningChips.Count;
                        tablestr = string.Format("{0}", goodRatio); // 양품률
                        tableResult[3].text = tablestr;

                        float fCost = float.Parse(cost.text);
                        float fPrice = float.Parse(price.text);
                        float benefit = (float)fPrice * (float)goodCount - (float)fCost * (float)runningChips.Count;
                        tablestr = string.Format("{0}", benefit); // 수익
                        tableResult[4].text = tablestr;

                        float benefitPerTime = benefit / Time;
                        tablestr = string.Format("{0}", benefitPerTime); // 시간당 수입
                        tableResult[5].text = tablestr;
                    }
                }
            }
            float DefectiveRatio = ((defectiveCount + notApplicableCount)*100) / TotalAmount;
            Debug.Log("DefectiveRatio: "+DefectiveRatio);
        }

        public void CreateChipTable()
        {
            int rows = runningChips.Count;
            int columns = 8; // MachineNumber, ProcessingState, QualityState, InsideState
            string[] titles = { "생성번호","투입기계 번호", "반출기", "투입기", "가공기", "공정단계", "품질상태", "내용물" };
            string[,] contents = new string[rows, columns];  
             
            int Check;
            // chips 리스트를 순회하면서 각 Chip 객체의 정보를 contents 배열에 채움
            for (int i = 0; i < rows; i++)
            {
                contents[i, 0] = runningChips[i].GetMachineNumber(0).ToString();
                contents[i, 1] = runningChips[i].GetMachineNumber(1).ToString();
                for(int a =1; a<4;a++ )
                {
                    Check = int.Parse(runningChips[i].GetMachineNumber(a).ToString());
                    if (Check >0)
                    {
                        contents[i, a+1] = "TRUE";
                    }
                    else if(Check == 0)
                    {
                        contents[i, a+1] = "FALSE";
                    }
                }                
                contents[i, 5] = runningChips[i].GetProcessingState().ToString();
                contents[i, 6] = runningChips[i].GetQualityState().ToString();
                contents[i, 7] = runningChips[i].GetInsideState().ToString();
            }           

            // 셀의 높이 설정 (모든 셀의 높이는 동일하게 설정) 
            float cellHeight = 200f;

            // 테이블 생성 호출
            tableCreator.CreateTable(rows, columns, titles, contents, cellHeight);
        }

        public void ReceiveData(List<AIReceiveData> datas)
        {
            
            tab.ActivatePanel(0);
            //AIVewer.gameObject.SetActive(true);
            AIButon.SetActive(true);
            aiButton1.Select();


            HeserBar.SetActive(false);
            Debug.Log("ReceiveData" + datas.Count);
            MainValue.Instance.aiViwer = datas;         
            
        }
    }
}
