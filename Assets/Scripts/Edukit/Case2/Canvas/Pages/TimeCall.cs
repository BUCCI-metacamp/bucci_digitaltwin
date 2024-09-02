using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeCall : MonoBehaviour
{
    public static TimeCall Instance { get; set; }

    public class TimeCallElement
    {   public bool IsEveryUpdate { get; set; }
        public float TimeRemain { get; set; }
        public UnityAction Callback { get; set; }       
        public void SetRemove() { TimeRemain = -1.0f; }
    }
    public List<TimeCallElement> listCall = new List<TimeCallElement>();
    public List<TimeCallElement> listUpdateFisrtLayer = new List<TimeCallElement>();
    public List<TimeCallElement> listUpdateSecondLayer = new List<TimeCallElement>();

    private void Awake()
    {
        Instance = this;
    }
    public void AddEvent( float time, UnityAction callBack )
    {
        listCall.Add(
            new TimeCallElement() 
            {   IsEveryUpdate = false,
                TimeRemain = time,
                Callback = callBack 
            });
    }
    public TimeCallElement AddAlwaysCall(UnityAction callBack)
    {
        var element = new TimeCallElement()
        {   IsEveryUpdate = true,
            TimeRemain = 0.0f,
            Callback = callBack
        };
        listCall.Add(element);
        return element;
    }
    public TimeCallElement AddFirstUpdate( UnityAction callBack)
    {
        var timeCall = new TimeCallElement()
        {   IsEveryUpdate = false,
            TimeRemain = 0.0f,            
            Callback = callBack
        };
        listUpdateFisrtLayer.Add(timeCall );
        return timeCall;
    }
    public TimeCallElement AddSecondUpdate(UnityAction callBack)
    {
        var timeCall = new TimeCallElement()
        {
            IsEveryUpdate = false,
            TimeRemain = 0.0f,
            Callback = callBack
        };
        listUpdateSecondLayer.Add(timeCall);
        return timeCall;
    }

    void Update()
    {
        for( int i=0;i< listCall.Count;++i)
        {
            var listOne = listCall[i];

            if (listOne.IsEveryUpdate == true)
                listOne.Callback.Invoke();
            else
            {
                listOne.TimeRemain -= Time.deltaTime;
                if (listOne.TimeRemain < 0.0f)
                    listOne.Callback.Invoke();
            }
        }
        listCall.RemoveAll( data => data.TimeRemain < 0.0f );


        for (int i = 0; i < listUpdateFisrtLayer.Count; ++i)
        {
            var listOne = listUpdateFisrtLayer[i];            
            listOne.Callback.Invoke();
        }

        listUpdateFisrtLayer.RemoveAll(data => data.TimeRemain < 0.0f);

        for (int i = 0; i < listUpdateSecondLayer.Count; ++i)
        {
            var listOne = listUpdateSecondLayer[i];
            listOne.Callback.Invoke();
        }

        listUpdateSecondLayer.RemoveAll(data => data.TimeRemain < 0.0f);
    }
}
