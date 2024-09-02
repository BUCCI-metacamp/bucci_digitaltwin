using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timer : UnExposedSingletone<Timer>
{
    List<Element> listElements = new List<Element>();
    List<Element> listLateElements = new List<Element>();
    List<Element> listAdd = new List<Element>();
    List<Element> listLateAdd = new List<Element>();
    List<Element> listRemoves = new List<Element>();
    Dictionary<string, Element> mapElement = new Dictionary<string, Element>();
    public bool IsClearReserved { get; private set; }
    static int CurrentGenerateId;


    public class Element
    {
        public int Id { get; private set; }
        public int Category { get; set; }
        public bool IsRepeat { get; private set; }
        public AnimationCurve Curve { get; private set; }
        public bool IsActive { get; private set; }
        public float Multiply { get; private set; }
        public int Count { get; private set; }
        public float Time { get; private set; }
        public float TargetTime { get; private set; }
        public System.Action<Element, float> Callback { get; private set; }
        public System.Action<Element, int> CallbackComplete { get; private set; }
        public Timer Manager { get; private set; }
        public System.Action<float> Update { get; private set; }
   

        public Element(Timer manager, int id, AnimationCurve curve, bool isRepeat, float target, float multi, System.Action<Element, float> callback, System.Action<Element, int> callbackComplete)
        {
            this.Manager = manager;
            this.Id = id;
            this.IsRepeat = isRepeat;
            this.Multiply = multi;
            this.Curve = curve;
            this.Callback = (callback == null) ? delegate (Element ele, float t) { } : callback;
            this.CallbackComplete = (callbackComplete == null) ? delegate (Element ele, int count) { } : callbackComplete;
            this.IsActive = true;
            this.Time = 0.0f;
            this.TargetTime = target;
            this.Count = 0;

            if (curve == null) this.Update = UpdateTime;
            else this.Update = UpdateCurve;
        }
        public void Remove()
        {
            this.Update = delegate (float t) { };

            Manager.RemoveTimer(this);
        }
        public void Clear()
        {
            this.Update = null;
            this.Callback = null;
            this.CallbackComplete = null;
        }
        public void UpdateCurve(float deltaTime)
        {
            Time += deltaTime;

            //Debug.Log("-->" + deltaTime + "/" + Time);
            if (Curve.keys[Curve.keys.Length - 1].time < Time)
            {
                var value = Curve.Evaluate(Time);
                Callback.Invoke(this, value * Multiply);
                CallbackComplete.Invoke(this, Count++);

                if (IsRepeat == false)
                    Remove();
                else
                    Time = 0.0f;
            }
            else
            {
                var value = Curve.Evaluate(Time);
                Callback.Invoke(this, value);
            }
        }
        public void UpdateTime(float deltaTime)
        {
            //Debug.Log("-->" + deltaTime + "/" + Time);
            Time += deltaTime;

            if (Time > TargetTime)
            {
                CallbackComplete.Invoke(this, Count++);
                if (IsRepeat == false)
                    Remove();
                else
                    Time = 0.0f;
            }
            else
                Callback.Invoke(this, Time * Multiply);
        }
    }
    void Awake()
    {
        CurrentGenerateId = 0;
    }
    public Element CurveSingle(AnimationCurve curve, System.Action<Element, float> callback, System.Action<Element, int> callbackComplete)
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, curve, false, 0.0f, 1.0f, callback, callbackComplete);

        listAdd.Add(newElement);
        return newElement;
    }
    public Element CurveSingleMultiply(AnimationCurve curve, float multi, System.Action<Element, float> callback, System.Action<Element, int> callbackComplete)
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, curve, false, 0.0f, multi, callback, callbackComplete);

        listAdd.Add(newElement);
        return newElement;
    }
    public Element CurveRepeat(AnimationCurve curve, System.Action<Element, float> callback, System.Action<Element, int> callbackComplete)
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, curve, true, 0.0f, 1.0f, callback, callbackComplete);

        listAdd.Add(newElement);
        return newElement;
    }
    public Element RepeatTime(float time, System.Action<Element, int> callbackComplete)
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, null, true, time, 1.0f, null, callbackComplete);

        listAdd.Add(newElement);
        return newElement;
    }
    public Element SingleTime(float time, System.Action<Element, int> callbackComplete)
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, null, false, time, 1.0f, null, callbackComplete);

        listAdd.Add(newElement);
        return newElement;
    }
    public Element EveryUpdateFrame(System.Action<Element,float> callback )
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, null, false, 100000, 1.0f,
            delegate (Element element, float time ) { callback.Invoke(element, time); },
            null
            );

        listAdd.Add(newElement);
        return newElement;
    }
    public Element EveryUpdateFrame(System.Action callback)
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, null, false, 100000, 1.0f,
            delegate (Element element, float time) { callback.Invoke(); },
            null
            );

        listAdd.Add(newElement);
        return newElement;
    }
    public Element EveryLateUpdateFrame(System.Action<Element, float> callback)
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, null, false, 100000, 1.0f,
            delegate (Element element, float time) { callback.Invoke(element, time); },
            null
            );

        listLateAdd.Add(newElement);
        return newElement;
    }
    public Element TimeCall(float time, System.Action callbackComplete)
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, null, false, time, 1.0f, null,
            delegate (Element element, int count) { callbackComplete.Invoke(); }
            );

        listAdd.Add(newElement);
        return newElement;
    }
    public Element TimeCall( string name, float time, System.Action callbackComplete)
    {
        Element newElement
            = new Element(this, ++CurrentGenerateId, null, false, time, 1.0f, null,
            delegate (Element element, int count) { callbackComplete.Invoke(); }
            );

        if (mapElement.ContainsKey(name) == true)
        {
            RemoveTimer(mapElement[name]);
            mapElement[name] = newElement;
        }
        else
            mapElement.Add(name, newElement);

        listAdd.Add(newElement);
        return newElement;
    }
    public void StopCall( string name )
    {
        if( mapElement.ContainsKey(name ) == true  )
            RemoveTimer(mapElement[name]);
    }
    public void ClearCategory( int category )
    {
        foreach (var listOne in listElements)
            if (listOne.Category == category)
                RemoveTimer(listOne);

    }

    void Update()
    {
        CleanUp();
        foreach (var listOne in listElements)            
                listOne.Update(Time.unscaledDeltaTime);

        AddUp();
    }
    void LateUpdate()
    {
        foreach (var listOne in listLateElements)
            listOne.Update(Time.unscaledDeltaTime);

        listLateElements.AddRange(listLateAdd);

        if (IsClearReserved == true)
        {
            listElements.Clear();
            listLateElements.Clear();
            listAdd.Clear();
            listLateAdd.Clear();
            listRemoves.Clear();
            mapElement.Clear();
            IsClearReserved = false;
            Debug.Log("Clear ");            
        }
    }
    public void Clear()
    {
        IsClearReserved = true;
    }
    public void RemoveTimer(Element target)
    {
        if (target == null) return;
        listRemoves.Add(target);
    }
    public void AddUp()
    {
        for( int i=0;i<listAdd.Count;++i)
        {
            if( listAdd[i].Update == null )
            {
                Debug.Log("====>Update NULL==>" + listAdd[i].Id  );
            }
        }

        listElements.AddRange(listAdd);
        listAdd.Clear();
    }
    public void CleanUp()
    {
        for( int i=0; i< listElements.Count;++i )
            if (listElements[i].Update == null)
                listRemoves.Add(listElements[i]);

        if (listRemoves.Count > 0)
        {
            foreach (var listOne in listRemoves)
            {
                listOne.Clear();
                listElements.RemoveAll(data => data == listOne);
            }
            listRemoves.Clear();
        }
    }
}
