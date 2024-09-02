using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HMIInputLimit : MonoBehaviour
{
   
    public TextMeshProUGUI VisionDice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        VisionDice.text = MainValue.Instance.InputLimit;
    }
}
