using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HMIEmergency : MonoBehaviour
{
  

    private Color[] colors = { Color.green, Color.blue, Color.yellow, Color.black }; // ������ ���� �迭    
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
            // ��: ������
            emergency.color = Color.black; // RGB ���� ��� 
            emergencyTXT.gameObject.SetActive(false);
        }
        else
        {
            // ��: ������            
            emergency.color = Color.red; // RGB ���� ���
            emergencyTXT.gameObject.SetActive(true);

        }
    }
}
