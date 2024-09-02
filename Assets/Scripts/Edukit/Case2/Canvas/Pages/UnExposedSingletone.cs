using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnExposedSingletone<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance = null;    

    public static bool IsInstacne
    {
        get { return instance != null;  }
    }
    public static T Instance
    {
        get
        {
            if (instance == null)
             instance = new GameObject(typeof(T).ToString()).AddComponent<T>();            
            return instance;
        }
    }
    void OnDestroy()
    {   
        Debug.Log("============Clear Called " + name );

        if (instance != null)
            Destroy(instance.gameObject);

        instance = null;
    }

}

