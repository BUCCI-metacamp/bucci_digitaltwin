using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HMIM03State : MonoBehaviour
{
    public TextMeshProUGUI No3Count;
    public TextMeshProUGUI No3O1st;
    public TextMeshProUGUI No3O2st;
    public TextMeshProUGUI Gripper;
    
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        No3Count.text = MainValue.Instance.No3Count;
        No3O1st.text = MainValue.Instance.No3Motor1Position;
        No3O2st.text = MainValue.Instance.No3Motor2Position;
        Gripper.text = MainValue.Instance.No3Gripper;

    }
}
