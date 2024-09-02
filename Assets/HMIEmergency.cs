using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HMIEmergency : MonoBehaviour
{
  

    private Color[] colors = { Color.green, Color.blue, Color.yellow, Color.black }; // 변경할 색상 배열    
    public Image emergency;
    public GameObject emergencyTXT;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MainValue.Instance.EmergencyState == "true")
        {
            // 예: 빨간색
            emergency.color = Color.black; // RGB 값을 사용 
            emergencyTXT.gameObject.SetActive(false);
        }
        else
        {
            // 예: 빨간색            
            emergency.color = Color.red; // RGB 값을 사용
            emergencyTXT.gameObject.SetActive(true);

        }
    }
}
