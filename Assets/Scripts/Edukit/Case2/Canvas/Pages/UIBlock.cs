

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class UIBlock : MonoBehaviour
{   
    bool isInside { get; set; }
    public bool IsIn { get { return isInside; } }

    void Start()
    {
        
    }
    void OnDisable()
    {
        isInside = false;
    }
    void OnMouseDown()
    {
       // PageMessage.Instance.Show("DD", "mousedown");
    }  
    void OnMouseUp()
    {        
    }
    void OnMouseDrag()
    {
    }
    private void OnMouseOver()
    {
        isInside = true;        
    }
    private void OnMouseExit()
    {
        isInside = false;
    }


}