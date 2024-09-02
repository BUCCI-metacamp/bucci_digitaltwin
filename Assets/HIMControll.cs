using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HIMControll : MonoBehaviour
{
    public List<Button> ControllButton = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        ControllButton.Add(GameObject.Find("startState").GetComponent<Button>());

    }
   
    // Update is called once per frame
    void Update()
    {
        
    }

    
}
